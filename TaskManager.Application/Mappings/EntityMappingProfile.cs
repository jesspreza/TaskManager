using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Domain.Entities;
using Task = TaskManager.Domain.Entities.Task;

namespace TaskManager.Application.Mappings
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile() 
        {
            CreateMap<CollaboratorDto, Collaborator>().ReverseMap()
                .ForMember(dest => dest.UserDto, opt => opt.MapFrom(x => x.User))
                .ForMember(dest => dest.TimeTrackersDto, opt => opt.MapFrom(x => x.TimeTrackers));

            CreateMap<CollaboratorRequestDto, Collaborator>().ReverseMap();
            CreateMap<CollaboratorResponseDto, Collaborator>().ReverseMap()
                .ForMember(dest => dest.UserResponseDto, opt => opt.MapFrom(x => x.User))
                .ForMember(dest => dest.TimeTrackersResponseDto, opt => opt.MapFrom(x => x.TimeTrackers));

            CreateMap<ProjectDto, Project>().ReverseMap()
                .ForMember(dest => dest.TasksDto, opt => opt.MapFrom(x => x.Tasks));

            CreateMap<ProjectRequestDto, Project>().ReverseMap();
            CreateMap<ProjectResponseDto, Project>().ReverseMap();
                //.ForMember(dest => dest.TasksResponseDto, opt => opt.MapFrom(x => x.Tasks));

            CreateMap<TaskDto, Task>().ReverseMap()
                .ForMember(dest => dest.ProjectDto, opt => opt.MapFrom(x => x.Project))
                .ForMember(dest => dest.TimeTrackersDto, opt => opt.MapFrom(x => x.TimeTrackers));

            CreateMap<TaskRequestDto, Task>().ReverseMap()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(x => x.ProjectId));
            CreateMap<TaskResponseDto, Task>().ReverseMap()
                .ForMember(dest => dest.ProjectResponseDto, opt => opt.MapFrom(x => x.Project))
                .ForMember(dest => dest.TimeTrackersResponseDto, opt => opt.MapFrom(x => x.TimeTrackers));

            CreateMap<TimeTrackerDto, TimeTracker>().ReverseMap()
                .ForMember(dest => dest.CollaboratorDto, opt => opt.MapFrom(x => x.Collaborator))
                .ForMember(dest => dest.TaskDto, opt => opt.MapFrom(x => x.Task));

            CreateMap<TimeTrackerRequestDto, TimeTracker>().ReverseMap();
            CreateMap<TimeTrackerResponseDto, TimeTracker>().ReverseMap()
                .ForMember(dest => dest.CollaboratorResponseDto, opt => opt.MapFrom(x => x.Collaborator))
                .ForMember(dest => dest.TaskResponseDto, opt => opt.MapFrom(x=> x.Task));
            
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserRequestDto, User>().ReverseMap();
            CreateMap<UserResponseDto, User>().ReverseMap();
        }
    }
}
