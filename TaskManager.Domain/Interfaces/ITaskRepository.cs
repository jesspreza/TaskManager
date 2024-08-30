using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository : IBaseRepository<Task, Guid>
    {
    }
}
