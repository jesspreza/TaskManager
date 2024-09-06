using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Data.Context;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infra.Data.Repositories
{
    public class TaskRepository : BaseRepository<Task, Guid>, ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Task> _dbSet;

        public TaskRepository(ApplicationDbContext dbContext ) : base(dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Task>();
        }

        public override async Task<IEnumerable<Task>> GetManyAsync(Expression<Func<Task, bool>> predicate)
        {
            return await _dbSet
                .Include(t => t.Project)
                .Include(t => t.TimeTrackers)
                    .ThenInclude(tt => tt.Collaborator)
                .Where(predicate)
                .ToListAsync();
        }
    }
}
