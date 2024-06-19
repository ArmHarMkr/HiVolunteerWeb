using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using HiVolunteerWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HiVolunteerWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext Context;
        private readonly UserManager<AppUser> UserManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<AppUser> userManager)
        {
            _logger = logger;
            Context = context;
            UserManager = userManager;  
        }

        public async Task<IActionResult> Index()
        {
            var user = await UserManager.GetUserAsync(User);
            bool isAccepted = user.IsAccepted;
            IEnumerable<VolunteeringEntity> allAllowedVolunteerings = Context.Volunteerings.Where(v => v.DeadLineDate < DateTime.Now).Where(v => v.IsNeededAccept == isAccepted);
            return View(allAllowedVolunteerings);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyToVolunteering(Guid id)
        {
            var user = await UserManager.GetUserAsync(User);
            bool isAccepted = user.IsAccepted;
            VolunteeringEntity? applyingVolunteering = await Context.Volunteerings.FirstOrDefaultAsync(c => c.Id == id);
            if(applyingVolunteering == null)
            {
                return BadRequest("No volunteering found. Try again");
            }
            if(applyingVolunteering.IsNeededAccept != isAccepted)
            {
                TempData["ErrorMessage"] = "You are not allowed to apply for this volunteering";
                return RedirectToAction("Index");
            }
            if(!await Context.WorkApplies.AnyAsync(c => c.AppliedUser == user))
            {
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
            TempData["ErrorMessage"] = "You have applied for this volunteering or something went wrong.";
            return RedirectToAction("Index");


        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
