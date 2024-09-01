using System.Linq.Expressions;
using TaskManager.Domain.Pagination;

namespace TaskManager.Application.Interfaces
{
    public interface IBaseService<TEntity, TKey, TRequestDto, TResponseDto>
    {
        Task<PagedList<TResponseDto>> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<TResponseDto> GetByIdAsync(TKey id);
        Task<TResponseDto> CreateAsync(TRequestDto dto);
        Task<TResponseDto> UpdateAsync(TKey id, TRequestDto dto);
        Task<TResponseDto> DeleteAsync(TKey id);
        Task<IEnumerable<TResponseDto>> GetMany(Expression<Func<TEntity, bool>> predicate);
        Task<TResponseDto> GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
    }
}
