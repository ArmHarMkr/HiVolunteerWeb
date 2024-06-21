using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using HiVolunteerWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

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
                Context.Remove(deletingVol);
                await Context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Volunteering deleted successfully";
                return RedirectToAction("AllVolunteerings");
            }
            TempData["ErrorMessage"] = "Volunteering deleted successfully";
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
            await NotificationService.SendNotification(id.ToString(), "Congratulations!", "Your application was approved", NotificationResponse.Success);
            await VolunteeringService.AcceptApplication(id);
            await Context.SaveChangesAsync();

            return RedirectToAction("AllAppliedVolunteerings");
        }
    }
}
