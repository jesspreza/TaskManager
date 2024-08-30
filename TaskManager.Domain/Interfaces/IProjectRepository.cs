using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IProjectRepository : IBaseRepository<Project, Guid>
    {
    }
}
