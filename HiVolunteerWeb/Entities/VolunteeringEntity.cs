using HiVolunteerWeb.Entity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HiVolunteerWeb.Entities
{
    public class VolunteeringEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Enter Name")]
        public string VolunteeringName { get; set;}
        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set;}
        [Required(ErrorMessage = "Enter the Address")]
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Enter Volunteering date")]
        public DateTime VolunteeringDate { get; set; }
        [Required(ErrorMessage = "Enter Deadline date")]
        public DateTime DeadLineDate { get; set; }
        [Required(ErrorMessage = "Check the box")]
        public bool IsNeededAccept { get; set; }
        [Required(ErrorMessage = "Check the box")]
        public bool IsFoodAvailable { get; set; }
        public bool IsActive { get; set; } = true;
        [AllowNull]
        public List<AppUser>? RegisteredUsers { get; set; } = new List<AppUser>();
    }
}
