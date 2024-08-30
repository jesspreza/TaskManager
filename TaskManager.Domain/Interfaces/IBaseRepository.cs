using System.Linq.Expressions;
using TaskManager.Domain.Pagination;

namespace TaskManager.Domain.Interfaces
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity?> DeleteByIdAsync(TKey id);
        Task<PagedList<TEntity>> GetAllPagedAsync(int pageNumber, int pageSize);

        Task<IEnumerable<TEntity?>> GetManyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
