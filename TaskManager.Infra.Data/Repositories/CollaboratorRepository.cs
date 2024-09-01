using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Data.Context;

namespace TaskManager.Infra.Data.Repositories
{
    public class CollaboratorRepository : BaseRepository<Collaborator, Guid>, ICollaboratorRepository
    {
        public CollaboratorRepository(ApplicationDbContext dbContext): base(dbContext)
        {
        }
    }
}
