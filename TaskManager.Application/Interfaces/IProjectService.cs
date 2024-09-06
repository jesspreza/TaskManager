using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Pagination;

namespace TaskManager.Application.Interfaces
{
    public interface IProjectService : IBaseService<Project, Guid, ProjectRequestDto, ProjectResponseDto>
    {
        Task<PagedList<ProjectResponseDto>> SearchProjectsAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
