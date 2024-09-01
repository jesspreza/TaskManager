using AutoMapper;
using System.Linq.Expressions;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Services
{
    public class BaseService<TEntity, TKey, TRequestDto, TResponseDto> where TEntity : class
    {
        private readonly IBaseRepository<TEntity, TKey> _repository;
        private readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity, TKey> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<PagedList<TResponseDto>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            var entities = await _repository.GetAllPagedAsync(pageNumber, pageSize);
            var dtos = _mapper.Map<IEnumerable<TResponseDto>>(entities);
            return new PagedList<TResponseDto>(dtos, pageNumber, pageSize, entities.TotalCount);
        }

        public virtual async Task<TResponseDto> GetByIdAsync(TKey id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            return _mapper.Map<TResponseDto>(entity);
        }

        protected virtual void PreCreate(TEntity entity, TRequestDto dto)
        {
        }

        public virtual async Task<TResponseDto> CreateAsync(TRequestDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            PreCreate(entity, dto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TResponseDto>(createdEntity);
        }

        protected virtual void PreUpdate(TEntity entity, TRequestDto dto)
        {
        }

        public virtual async Task<TResponseDto> UpdateAsync(TKey id, TRequestDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            _mapper.Map(dto, entity);

            PreUpdate(entity, dto);

            await _repository.UpdateAsync(entity);
            return _mapper.Map<TResponseDto>(entity);
        }

        public virtual async Task<TResponseDto> DeleteAsync(TKey id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            entity = await _repository.DeleteByIdAsync(id);
            return _mapper.Map<TResponseDto>(entity);
        }

        public async Task<IEnumerable<TResponseDto?>> GetMany(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _repository.GetManyAsync(predicate);
            return _mapper.Map<IEnumerable<TResponseDto>>(entities);
        }

        public async Task<TResponseDto?> GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _repository.GetFirstOrDefaultAsync(predicate);
            return _mapper.Map<TResponseDto>(entity);
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return await _repository.ExistsAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _repository.CountAsync(predicate);
        }
    }
}
