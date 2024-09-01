using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Data.Context;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infra.Data.Repositories
{
    public class TaskRepository : BaseRepository<Task, Guid>, ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
