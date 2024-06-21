using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using HiVolunteerWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HiVolunteerWeb.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private AppDbContext Context;
        private UserManager<AppUser> UserManager;
        private INotificationService NotificationService;

        public NotificationController(AppDbContext db,
            UserManager<AppUser> userManager,
            INotificationService notificationService)
        {
            Context = db;
            UserManager = userManager;
            NotificationService = notificationService;
        }

        public async Task<IActionResult> AllNotifications()
        {
            var currentUser = await UserManager.GetUserAsync(User);
            IEnumerable<NotificationEntity> notifications = await Context.Notifications.Include(c => c.NotificationSendingUser).Where(c => c.NotificationSendingUser == currentUser).ToListAsync();
            return View(notifications);
        }
    }
}
