using AutoMapper;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Pagination;

namespace TaskManager.Application.Services
{
    public class ProjectService : BaseService<Project, Guid, ProjectRequestDto, ProjectResponseDto>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
            : base(projectRepository, mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        protected override void PreUpdate(Project entity, ProjectRequestDto dto)
        {
            entity.UpdateDomain(dto.Name);
        }

        public async Task<PagedList<ProjectResponseDto>> SearchProjectsAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            IEnumerable<Project> filteredProjects;
            int totalCount;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                filteredProjects = await _projectRepository.GetManyAsync(p =>
                    p.Name.Contains(searchTerm) &&
                   !p.DeletedAt.HasValue);
            }
            else
            {
                filteredProjects = await _projectRepository.GetManyAsync(p =>
                    !p.DeletedAt.HasValue);
            }

            totalCount = filteredProjects.Count();

            var pagedProjects = filteredProjects.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var projectDtos = _mapper.Map<IEnumerable<ProjectResponseDto>>(pagedProjects);

            return new PagedList<ProjectResponseDto>(projectDtos.ToList(), totalCount, pageNumber, pageSize);
        }
    }
}
