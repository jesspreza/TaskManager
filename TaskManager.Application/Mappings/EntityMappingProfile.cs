using AutoMapper;
using TaskManager.Application.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Mappings
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile() 
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
