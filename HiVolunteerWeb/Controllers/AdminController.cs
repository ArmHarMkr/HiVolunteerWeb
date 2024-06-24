using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using HiVolunteerWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Extensibility;

namespace HiVolunteerWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private AppDbContext Context;
        private UserManager<AppUser> UserManager;
        private SignInManager<AppUser> SignInManager;
        private IActionsWithVolunteers UserActions;
        private IVolunteeringService VolunteeringService;
        private INotificationService NotificationService;

        public AdminController(AppDbContext db, 
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

        public async Task<IActionResult> AllVolunteerings()
        {
            return View(await Context.Volunteerings.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddVolunteering()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVolunteering(VolunteeringEntity volunteering)
        {
            if (ModelState.IsValid)
            {
                Context.Volunteerings.Add(volunteering);
                await Context.SaveChangesAsync();
                return RedirectToAction("AllVolunteerings");
            }
            return View(volunteering);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVolunteering(Guid id)
        {
            VolunteeringEntity deletingVol = await Context.Volunteerings.FirstOrDefaultAsync(c => c.Id == id);
            if(deletingVol != null)
            {
                deletingVol.IsActive = false;
                await Context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Volunteering deleted successfully";
                return RedirectToAction("AllVolunteerings");
            }
            TempData["ErrorMessage"] = "Volunteering was not found";
            return RedirectToAction("AllVolunteerings");
        }

        [HttpGet]
        public async Task<IActionResult> EditVolunteering(Guid id)
        {
            VolunteeringEntity editingVolunteering = await Context.Volunteerings.FirstOrDefaultAsync(c => c.Id == id);
            if(editingVolunteering != null)
            {
                return View(editingVolunteering);
            }
            TempData["ErrorMessage"] = "No Volunteering found";
            return RedirectToAction("AllVolunteerings");
        }

        [HttpPost]
        public async Task<IActionResult> EditVolunteering(VolunteeringEntity volunteering)
        {
            if (ModelState.IsValid)
            {
                Context.Update(volunteering);
                await Context.SaveChangesAsync();
                return RedirectToAction("AllVolunteerings");
            }
            TempData["ErrorMessage"] = "No Volunteering found";
            return RedirectToAction("EditVolunteering", new {id = volunteering.Id});
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            return View(await Context.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AcceptVolunteer(string id)
        {
            var acceptingUser = await UserManager.Users.FirstOrDefaultAsync(c => c.Id == id);
            NotificationEntity notification = new()
            {
                NotificationSendingUser = acceptingUser,
                NotificationSendingUserId = acceptingUser.Id,
                Title = "You have been accepted",
                Description = "Your application to become a volunteer has been accepted",
                NotificationResponse = NotificationResponse.Success
            };

            await Context.Notifications.AddAsync(notification);
            await UserActions.AcceptVolunteer(id);
            await Context.SaveChangesAsync();

            return RedirectToAction("AllUsers");
        }

        [HttpGet]
        public async Task<IActionResult> AllAppliedVolunteerings()
        {
            IEnumerable<WorkApplies> allApplications = VolunteeringService.GetAllApplications().OrderByDescending(c => c.AppliedDate);
            return View(allApplications);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptApplication(Guid id)
        {
            var acceptingWorkApplication = await Context.WorkApplies.Include(c => c.AppliedUser).Include(c => c.Volunteering).FirstOrDefaultAsync(c => c.Id == id);
            NotificationEntity notification = new()
            {
                NotificationSendingUser = acceptingWorkApplication.AppliedUser,
                NotificationSendingUserId = acceptingWorkApplication.AppliedUser.Id,
                Title = "You work application has been accepted",
                Description = "Your application was accepted. You can see all your applications on the AllApplications Service",
                NotificationResponse = NotificationResponse.Success
            };
            acceptingWorkApplication.Volunteering.RegisteredUsers.Add(acceptingWorkApplication.AppliedUser);
            await VolunteeringService.AcceptApplication(id);
            Context.Notifications.Add(notification);
            await Context.SaveChangesAsync();

            return RedirectToAction("AllAppliedVolunteerings");
        }

        [HttpPost]
        public async Task<IActionResult> RejectAplication(Guid id)
        {
            var rejectingApplication = await Context.WorkApplies.Include(c => c.AppliedUser).Include(c => c.Volunteering).FirstOrDefaultAsync(c => c.Id == id);
            if(rejectingApplication == null)
            {
                TempData["ErrorMessage"] = "No volunteering found";
                return RedirectToAction("AllAppliedVolunteerings");
            }
            NotificationEntity notification = new()
            {
                NotificationSendingUser = rejectingApplication.AppliedUser,
                NotificationSendingUserId = rejectingApplication.AppliedUser.Id,
                Title = "You work application has been rejected",
                Description = "Your application was rejected. You can see all your applications on the AllApplications Service",
                NotificationResponse = NotificationResponse.Alert
            };
            Context.WorkApplies.Remove(rejectingApplication);
            Context.Notifications.Add(notification);
            await Context.SaveChangesAsync();

            return RedirectToAction("AllAppliedVolunteerings");
        }

        [HttpGet]
        public async Task<IActionResult> GetVolunteeringInfo(Guid id)
        {
            var volunteering = await Context.Volunteerings.Include(c => c.RegisteredUsers).FirstOrDefaultAsync(c => c.Id == id);
            
            if(volunteering == null)
            {
                TempData["ErrorMessage"] = "Volunteering not found";
                return RedirectToAction("AllVolunteerings");
            }

            return View(volunteering);
        }

    }
}
