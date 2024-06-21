using HiVolunteerWeb.Entities;

namespace HiVolunteerWeb.Services
{
    public interface INotificationService
    {
        public Task SendNotification(string userId, string title, string description, NotificationResponse notificationResponse);
        public Task<IEnumerable<NotificationEntity>> GetAllNotificationForUser(string userId);
    }
}
