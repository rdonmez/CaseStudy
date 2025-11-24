using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NotificationService.API.Requests;
using NotificationService.Entity.Domain;
using NotificationService.Entity.Repositories;

namespace NotificationService.API.Handlers
{
    public class GetAllNotificationHandler: IRequestHandler<GetAllNotificationRequest, IEnumerable<Notification>>
    {
        private readonly INotificationRepository _repository;

        public GetAllNotificationHandler(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Notification>> Handle(GetAllNotificationRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllNotificationsAsync();
        }
    }
}