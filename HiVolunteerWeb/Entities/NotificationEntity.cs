using HiVolunteerWeb.Entity;

namespace HiVolunteerWeb.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; set; }
        public AppUser NotificationSendingUser { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationResponse NotificationResponse { get; set; }
    }

    public enum NotificationResponse
    {
        Success,
        Alert,
        Fail,
        Other
    }
}
