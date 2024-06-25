using HiVolunteerWeb.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HiVolunteerWeb.Entity
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Input your name")]
        public string FullName { get; set; }
        [AllowNull]
        public string AboutUser { get; set; }
        public bool IsAccepted { get; set; } = false;
        [AllowNull]
        public string MainCharacteristics { get; set; }
        public int VolunteeringCount { get; set; } = 0;
        public ICollection<NotificationEntity> Notifications { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}