using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITimeTrackerRepository : IBaseRepository<TimeTracker, Guid>
    {
    }
}
