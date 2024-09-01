using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IProjectService : IBaseService<Project, Guid, ProjectRequestDto, ProjectResponseDto>
    {
    }
}
