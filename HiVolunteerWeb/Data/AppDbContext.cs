using HiVolunteerWeb.Entities;
using HiVolunteerWeb.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HiVolunteerWeb.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<VolunteeringEntity> Volunteerings { get; set; }
        public DbSet<WorkApplies> WorkApplies { get; set; }
    }
}
