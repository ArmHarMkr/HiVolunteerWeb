using HiVolunteerWeb.Entity;

namespace HiVolunteerWeb.Entities
{
    public class WorkApplies
    {
        public Guid Id { get; set; }
        public AppUser AppliedUser { get; set; }
        public VolunteeringEntity Volunteering { get; set; }
        public bool IsAccepted { get; set; } = false;
        public DateTime AppliedDate { get; set; } = DateTime.Now;
    }
}
