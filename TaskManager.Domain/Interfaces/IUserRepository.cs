using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
    }
}
