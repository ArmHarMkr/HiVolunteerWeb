
using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HiVolunteerWeb.Services
{
    public class AcceptVolunteerService : IActionsWithVolunteers
    {
        private readonly AppDbContext Context;
        private readonly UserManager<AppUser> UserManager;   

        public AcceptVolunteerService(AppDbContext context, UserManager<AppUser> userManager)
        {
            Context = context;
            UserManager = userManager;
        }


        public async Task AcceptVolunteer(string id)
        {
            AppUser currentVolunteer = await Context.Users.FirstOrDefaultAsync(c => c.Id == id);
            if(currentVolunteer == null)
            {
                throw new Exception("No user found");
            }

            if (await UserManager.IsInRoleAsync(currentVolunteer, "Admin"))
            {
                throw new Exception("Cannot give admin volunteer role");
            }

            currentVolunteer.IsAccepted = true;
            Context.Update(currentVolunteer);
            await Context.SaveChangesAsync();
        }
    }
}
