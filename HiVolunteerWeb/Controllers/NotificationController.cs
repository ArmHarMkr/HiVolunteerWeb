using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiVolunteerWeb.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        public IActionResult AllNotifications()
        {
            return View();
        }
    }
}
