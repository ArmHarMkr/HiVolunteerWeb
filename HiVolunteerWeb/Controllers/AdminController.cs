using AspNetCore;
using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
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
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AdminController(AppDbContext db, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            Context = db;
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}
