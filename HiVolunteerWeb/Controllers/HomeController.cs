using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using HiVolunteerWeb.Models;
using HiVolunteerWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

namespace HiVolunteerWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private AppDbContext Context;
        private UserManager<AppUser> UserManager;
        private SignInManager<AppUser> SignInManager;
        private IActionsWithVolunteers UserActions;
        private IVolunteeringService VolunteeringService;
        private INotificationService NotificationService;

        public HomeController(AppDbContext db,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IActionsWithVolunteers actionsWithVolunteers,
            IVolunteeringService volunteeringService,
            INotificationService notificationService)
        {
            Context = db;
            UserManager = userManager;
            SignInManager = signInManager;
            UserActions = actionsWithVolunteers;
            VolunteeringService = volunteeringService;
            NotificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await UserManager.GetUserAsync(User);
            if (!await UserManager.IsInRoleAsync(user, "Admin"))
            {
                bool isAccepted = user.IsAccepted;
                IEnumerable<VolunteeringEntity> allAllowedVolunteerings;
                if (user.IsAccepted)
                {
                    allAllowedVolunteerings = Context.Volunteerings.Where(v => v.DeadLineDate < DateTime.Now).Where(c => c.IsActive);
                }
                else
                {
                    allAllowedVolunteerings = Context.Volunteerings.Where(v => v.DeadLineDate < DateTime.Now)
                        .Where(v => v.IsNeededAccept == isAccepted).Where(c => c.IsActive);
                }
                return View(allAllowedVolunteerings);
            }
            return RedirectToAction("AllVolunteerings", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> ApplyToVolunteering(Guid id)
        {
            var user = await UserManager.GetUserAsync(User);
            bool isAccepted = user.IsAccepted;
            VolunteeringEntity? applyingVolunteering = await Context.Volunteerings.FirstOrDefaultAsync(c => c.Id == id);

            if (applyingVolunteering == null)
            {
                return BadRequest("No volunteering found. Try again");
            }

            if (applyingVolunteering.IsNeededAccept != isAccepted)
            {
                TempData["ErrorMessage"] = "You are not allowed to apply for this volunteering";
                return RedirectToAction("Index");
            }

            var currentUser = await UserManager.GetUserAsync(User);

            if (applyingVolunteering.RegisteredUsers.Any(u => u.Id == user.Id))
            {
                TempData["ErrorMessage"] = "You have already applied for this volunteering.";
                return RedirectToAction("Index");
            }

            WorkApplies workApplies = new()
            {
                AppliedUser = user,
                Volunteering = applyingVolunteering
            };

            Context.WorkApplies.Add(workApplies);
            await Context.SaveChangesAsync();

            TempData["SuccessMessage"] = "You applied successfully";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> AllAppliedVolunteerings()
        {
            var currentUser = await UserManager.GetUserAsync(User);
            return View(VolunteeringService.GetAllApplications().Where(c => c.AppliedUser == currentUser));
        }

        [HttpGet]
        public async Task<IActionResult> GetVolunteerInformation()
        {
            var currentUser = await UserManager.GetUserAsync(User);

            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile(AppUser appUser)
        {
            var currentUser = await UserManager.GetUserAsync(User);

            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }

            currentUser.AboutUser = appUser.AboutUser;
            currentUser.MainCharacteristics = appUser.MainCharacteristics;

            currentUser.RowVersion = appUser.RowVersion;
            await Context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Profile updated successfully.";

            return RedirectToAction("Index");
        }

    }
}
