using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class CollaboratorService : BaseService<Collaborator, Guid, CollaboratorRequestDto, CollaboratorResponseDto>, ICollaboratorService
    {
        private readonly ICollaboratorRepository _collaboratorRepository;

        public CollaboratorService(ICollaboratorRepository collaboratorRepository, IMapper mapper)
            : base(collaboratorRepository, mapper)
        {
            _collaboratorRepository = collaboratorRepository;
        }

        protected override void PreUpdate(Collaborator entity, CollaboratorRequestDto dto)
        {
            entity.UpdateDomain(dto.Name);
        }
    }
}
