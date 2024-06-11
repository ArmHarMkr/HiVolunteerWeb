using HiVolunteerWeb.Entity;
using System.ComponentModel.DataAnnotations;

namespace HiVolunteerWeb.Entities
{
    public class VolunteeringEntity
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Enter Name")]
        public string VolunteeringName { get; set;}
        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set;}
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime VolunteeringDate { get; set; }
        public DateTime DeadLineDate { get; set; }
        public bool IsNeededAccept { get; set; }
        public bool IsFoodAvailable { get; set; } = true;
        public bool IsActive { get;set; }
        public int MaxPeopleCount { get; set; }
        public ICollection<AppUser> RegisteredUsers { get; set; }
    }
}
