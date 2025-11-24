using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using NotificationService.Event.Exceptions;

namespace NotificationService.Event
{
    public class EventManager : IAsyncDisposable
    {
        private const string ExchangeName = "main_exchange";
        private const string DeadLetterExchange = "dead_letter_exchange";
         
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        private EventManager(IConnection connection, IChannel channel)
        {
            _connection = connection;
            _channel = channel;
        }

        public static async Task<EventManager> CreateAsync(string hostname, string username, string password)
        {
            var factory = new ConnectionFactory
            {
                HostName = hostname,
                UserName = username,
                Password = password,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(60),
            };

            var connection = await factory.CreateConnectionAsync();
            
            var channel = await connection.CreateChannelAsync();

            return new EventManager(connection, channel);
        }

        public async Task DeclareExchangeAsync()
        {
            await _channel.ExchangeDeclareAsync(
                exchange: ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);
            
            await _channel.ExchangeDeclareAsync(
                exchange: DeadLetterExchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);
        }

        public async Task PublishAsync<T>(T @event, string routingKey, string queueName)
        {
            try
            {
                await _channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                
                await _channel.QueueBindAsync(
                    queue: queueName,
                    exchange: ExchangeName,
                    routingKey: routingKey);

                var message = JsonSerializer.Serialize(@event);
                var body = Encoding.UTF8.GetBytes(message);
                
                await _channel.BasicPublishAsync(
                    exchange: ExchangeName,
                    routingKey: routingKey,
                    body: body);
            }
            catch (MessageException e)
            {
                throw new MessageException("Failed to send message on RabbitMQ.", e);
            }
        }

        public async Task SubscribeAsync<T>(string queueName, string routingKey, Func<T, Task> onMessage)
        {
            await _channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            await _channel.QueueBindAsync(
                queue: queueName,
                exchange: ExchangeName,
                routingKey: routingKey);
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(body));

                    var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    await onMessage(message).WaitAsync(TimeSpan.FromSeconds(3), TimeProvider.System, cts.Token);

                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };
 
            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }

        public async ValueTask DisposeAsync()
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}