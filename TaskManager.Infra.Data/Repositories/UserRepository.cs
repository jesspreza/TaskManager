using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using TaskManager.Infra.Data.Context;
using TaskManager.Infra.Data.Helpers;

namespace TaskManager.Infra.Data.Repositories
{
    public class UserRepository : IBaseRepository<User, Guid>
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> AddAsync(User entity)
        {
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<User?> DeleteByIdAsync(Guid id)
        {
            var entity = await _dbContext.Users.FindAsync(id);
            if (entity == null)
                return null;

            _dbContext.Users.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedList<User>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var query = _dbContext.Users.AsQueryable();
            return await PaginationHelper.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<IEnumerable<User?>> GetManyAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.Where(predicate).ToListAsync();
        }

        public async Task<User?> GetFirstOrDefaultAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<User, bool>> predicate)
        {
            return await _dbContext.Users.CountAsync(predicate);
        }
    }
}
