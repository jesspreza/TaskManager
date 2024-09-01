using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Interfaces;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Services
{
    public class TaskService : BaseService<Task, Guid, TaskRequestDto, TaskResponseDto>, ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
            : base(taskRepository, mapper)
        {
            _taskRepository = taskRepository;
        }

        protected override void PreUpdate(Task entity, TaskRequestDto dto)
        {
            entity.UpdateDomain(dto.Name, dto.Description, dto.ProjectId);
        }
    }
}
