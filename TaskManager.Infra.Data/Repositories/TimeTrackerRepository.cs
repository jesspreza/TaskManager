using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using TaskManager.Infra.Data.Context;
using TaskManager.Infra.Data.Helpers;

namespace TaskManager.Infra.Data.Repositories
{
    public class TimeTrackerRepository : IBaseRepository<TimeTracker, Guid>
    {
        private readonly ApplicationDbContext _dbContext;

        public TimeTrackerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TimeTracker?> GetByIdAsync(Guid id)
        {
            return await _dbContext.TimeTrackers.FindAsync(id);
        }

        public async Task<TimeTracker> AddAsync(TimeTracker entity)
        {
            _dbContext.TimeTrackers.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TimeTracker> UpdateAsync(TimeTracker entity)
        {
            _dbContext.TimeTrackers.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TimeTracker?> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbContext.TimeTrackers.FindAsync(id);
            if (entity == null)
                return null;

            _dbContext.TimeTrackers.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedList<TimeTracker>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.TimeTrackers.AsQueryable();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<TimeTracker?>> GetManyAsync(Expression<Func<TimeTracker, bool>> predicate)
        {
            return await _dbContext.TimeTrackers.Where(predicate).ToListAsync();
        }

        public async Task<TimeTracker?> GetFirstOrDefaultAsync(Expression<Func<TimeTracker, bool>> predicate)
        {
            return await _dbContext.TimeTrackers.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TimeTracker, bool>> predicate)
        {
            return await _dbContext.TimeTrackers.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TimeTracker, bool>> predicate)
        {
            return await _dbContext.TimeTrackers.CountAsync(predicate);
        }
    }
}
