using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Utils;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IUserService : IBaseService<User, Guid, UserRequestDto, UserResponseDto>
    {
        Task<ServiceResult<UserResponseDto>> RegisterUserAsync(UserRequestDto userRequestDto);
        Task<ServiceResult<string>> LoginAsync(UserRequestDto userRequestDto);
    }
}
