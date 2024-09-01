using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ICollaboratorService : IBaseService<Collaborator, Guid, CollaboratorRequestDto, CollaboratorResponseDto>
    {
    }
}
