using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NotificationService.API.Requests;
using NotificationService.Entity.Domain;
using NotificationService.Entity.Repositories;

namespace NotificationService.API.Handlers
{
    public class GetNotificationHandler:  IRequestHandler<GetNotificationRequest, Notification>
    {
        private readonly INotificationRepository _repository;

        public GetNotificationHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Notification> Handle(GetNotificationRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetNotificationByIdAsync(request.Id);
        }
    }
}