using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult AddVolunteering()
        {
            return View();
        }

        public async Task<IActionResult> AddVolunteering(VolunteeringEntity volunteering)
        {
            if (ModelState.IsValid)
            {
                Context.Volunteerings.Add(volunteering);
                return RedirectToAction("AllVolunteerings");
            }
            return View(volunteering);
        }
    }
}
