using HiVolunteerWeb.Entities;

namespace HiVolunteerWeb.Services
{
    public interface IVolunteeringService
    {
        public IEnumerable<WorkApplies> GetAllApplications();
        public Task AcceptApplication(Guid applicationId);
    }
}
