using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Data.Context;

namespace TaskManager.Infra.Data.Repositories
{
    public class TimeTrackerRepository : BaseRepository<TimeTracker, Guid>, ITimeTrackerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TimeTracker> _dbSet;

        public TimeTrackerRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TimeTracker>();
        }
        public override async Task<IEnumerable<TimeTracker>> GetManyAsync(Expression<Func<TimeTracker, bool>> predicate)
        {
            return await _dbSet
                .Include(tt => tt.Collaborator)
                .Include(tt => tt.Task)
                .ThenInclude(t => t.Project)
                .Where(predicate).OrderByDescending(t => t.StartDate)
                .ToListAsync();
        }
    }
}
