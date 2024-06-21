
using HiVolunteerWeb.Data;
using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HiVolunteerWeb.Services
{
    public class VolunteeringService : IVolunteeringService
    {
        private readonly AppDbContext Context;
        private readonly UserManager<AppUser> UserManager;

        public VolunteeringService(AppDbContext context, UserManager<AppUser> userManager)
        {
            Context = context;
            UserManager = userManager;
        }

        public async Task AcceptApplication(Guid applicationId)
        {
            var workApp = await Context.WorkApplies.Include(c => c.AppliedUser).Include(c => c.Volunteering).FirstOrDefaultAsync(c => c.Id == applicationId);
            if(workApp == null)
            {
                throw new Exception("No application found");
            }

            workApp.Volunteering.RegisteredUsers.Add(workApp.AppliedUser);
            workApp.AppliedUser.VolunteeringCount++;
            workApp.IsAccepted = true;
            Context.WorkApplies.Update(workApp);
            await Context.SaveChangesAsync();
        }

        public IEnumerable<WorkApplies> GetAllApplications()
        {
            return Context.WorkApplies.Include(c => c.AppliedUser).Include(c => c.Volunteering);
        }
    }
}
