using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsAsync(int userId);
        Task<Notification> AddNotification(NotificationModal notificationModal);
    }
}
