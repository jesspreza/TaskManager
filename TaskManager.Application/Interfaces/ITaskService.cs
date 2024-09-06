using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Pagination;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskService : IBaseService<Task, Guid, TaskRequestDto, TaskResponseDto>
    {
        Task<PagedList<TaskListResponseDto>> SearchTasksAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
