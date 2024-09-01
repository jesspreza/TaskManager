using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class ProjectService : BaseService<Project, Guid, ProjectRequestDto, ProjectResponseDto>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
            : base(projectRepository, mapper)
        {
            _projectRepository = projectRepository;
        }

        protected override void PreUpdate(Project entity, ProjectRequestDto dto)
        {
            entity.UpdateDomain(dto.Name);
        }
    }
}
