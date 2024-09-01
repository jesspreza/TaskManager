using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskService : IBaseService<Task, Guid, TaskRequestDto, TaskResponseDto>
    {
    }
}
