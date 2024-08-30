using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using TaskManager.Infra.Data.Context;
using TaskManager.Infra.Data.Helpers;

namespace TaskManager.Infra.Data.Repositories
{
    public class CollaboratorRepository : IBaseRepository<Collaborator, Guid>
    {
        private readonly ApplicationDbContext _dbContext;

        public CollaboratorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Collaborator?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Collaborators.FindAsync(id);
        }

        public async Task<Collaborator> AddAsync(Collaborator entity)
        {
            _dbContext.Collaborators.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Collaborator> UpdateAsync(Collaborator entity)
        {
            _dbContext.Collaborators.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Collaborator?> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbContext.Collaborators.FindAsync(id);
            if (entity == null)
                return null;

            _dbContext.Collaborators.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedList<Collaborator>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.Collaborators.AsQueryable();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Collaborator?>> GetManyAsync(Expression<Func<Collaborator, bool>> predicate)
        {
            return await _dbContext.Collaborators.Where(predicate).ToListAsync();
        }

        public async Task<Collaborator?> GetFirstOrDefaultAsync(Expression<Func<Collaborator, bool>> predicate)
        {
            return await _dbContext.Collaborators.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Collaborator, bool>> predicate)
        {
            return await _dbContext.Collaborators.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<Collaborator, bool>> predicate)
        {
            return await _dbContext.Collaborators.CountAsync(predicate);
        }
    }
}
