using AutoMapper;
using Quote.Interfaces.RepositoryInterface;
using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;

namespace Quote.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepoBase<Notification> _repoNoti;
        private readonly IMapper _mapper;
        public NotificationService(IRepoBase<Notification> repoNoti, IMapper mapper)
        {
            _repoNoti = repoNoti;
            _mapper = mapper;
        }
        public async Task<List<Notification>> GetNotificationsAsync(int userId)
        {
            try
            {
               var data = await _repoNoti.GetAllAsync();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Notification> AddNotification(NotificationModal notificationModal)
        {
            try
            {
                var dataAdd = _mapper.Map<NotificationModal, Notification>(notificationModal);
                var dataRes = await _repoNoti.AddAsync(dataAdd);
                return dataRes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
