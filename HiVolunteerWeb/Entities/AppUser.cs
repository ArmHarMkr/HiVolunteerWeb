using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HiVolunteerWeb.Entity
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        [AllowNull]
        public string AboutUser { get; set; }
        public bool IsAccepted { get; set; } = false;
        [AllowNull]
        public string MainCharacteristics { get; set; }
        public int VolunteeringCount { get; set; } = 0;
    }
}