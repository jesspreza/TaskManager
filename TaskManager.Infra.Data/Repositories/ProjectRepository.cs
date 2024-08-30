using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using TaskManager.Infra.Data.Context;
using TaskManager.Infra.Data.Helpers;

namespace TaskManager.Infra.Data.Repositories
{
    public class ProjectRepository : IBaseRepository<Project, Guid>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Projects.FindAsync(id);
        }

        public async Task<Project> AddAsync(Project entity)
        {
            _dbContext.Projects.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Project> UpdateAsync(Project entity)
        {
            _dbContext.Projects.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Project?> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbContext.Projects.FindAsync(id);
            if (entity == null)
                return null;

            _dbContext.Projects.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedList<Project>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.Projects.AsQueryable();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Project?>> GetManyAsync(Expression<Func<Project, bool>> predicate)
        {
            return await _dbContext.Projects.Where(predicate).ToListAsync();
        }

        public async Task<Project?> GetFirstOrDefaultAsync(Expression<Func<Project, bool>> predicate)
        {
            return await _dbContext.Projects.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Project, bool>> predicate)
        {
            return await _dbContext.Projects.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<Project, bool>> predicate)
        {
            return await _dbContext.Projects.CountAsync(predicate);
        }
    }
}
