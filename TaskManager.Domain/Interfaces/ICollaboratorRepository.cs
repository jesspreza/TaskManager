using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ICollaboratorRepository : IBaseRepository<Collaborator, Guid>
    {
    }
}
