using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Data.Context;

namespace TaskManager.Infra.Data.Repositories
{
    public class TimeTrackerRepository : BaseRepository<TimeTracker, Guid>, ITimeTrackerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TimeTrackerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
