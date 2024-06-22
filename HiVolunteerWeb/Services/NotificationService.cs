using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HiVolunteerWeb.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext Context;
        private readonly UserManager<AppUser> UserManager;

        public NotificationService(AppDbContext context, UserManager<AppUser> userManager)
        {
            Context = context;
            UserManager = userManager;
        }

        public async Task SendNotification(string userID, string title, string description, NotificationResponse notificationResponse)
        {
            var currentUser = await UserManager.Users.FirstOrDefaultAsync(c => c.Id == userID);
            if (currentUser == null)
            {
                throw new Exception("User not found");
            }
            NotificationEntity notification = new()
            {
                NotificationSendingUserId = userID,
                NotificationSendingUser = currentUser,
                Title = title,
                Description = description,
                NotificationResponse = notificationResponse
            };

            Context.Notifications.Add(notification);
            await Context.SaveChangesAsync();
        }


        public async Task<IEnumerable<NotificationEntity>> GetAllNotificationForUser(string userId)
        {
            var currentUser = await UserManager.Users.FirstOrDefaultAsync(c => c.Id == userId);
            return await Context.Notifications.Include(c => c.NotificationSendingUser).Where(c => c.NotificationSendingUser == currentUser).ToListAsync();
        }
    }
}
