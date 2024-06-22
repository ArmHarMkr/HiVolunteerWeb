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
        public DbSet<NotificationEntity> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationEntity>()
                .HasOne(n => n.NotificationSendingUser)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.NotificationSendingUserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
