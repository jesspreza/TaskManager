using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using TaskManager.Infra.Data.Context;
using TaskManager.Infra.Data.Helpers;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infra.Data.Repositories
{
    public class TaskRepository : IBaseRepository<Task, Guid>
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Task?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Tasks.FindAsync(id);
        }

        public async Task<Task> AddAsync(Task entity)
        {
            _dbContext.Tasks.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Task> UpdateAsync(Task entity)
        {
            _dbContext.Tasks.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Task?> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbContext.Tasks.FindAsync(id);
            if (entity == null)
                return null;

            _dbContext.Tasks.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedList<Task>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.Tasks.AsQueryable();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Task?>> GetManyAsync(Expression<Func<Task, bool>> predicate)
        {
            return await _dbContext.Tasks.Where(predicate).ToListAsync();
        }

        public async Task<Task?> GetFirstOrDefaultAsync(Expression<Func<Task, bool>> predicate)
        {
            return await _dbContext.Tasks.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Task, bool>> predicate)
        {
            return await _dbContext.Tasks.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<Task, bool>> predicate)
        {
            return await _dbContext.Tasks.CountAsync(predicate);
        }
    }
}
