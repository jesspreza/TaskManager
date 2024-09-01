using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Utils;
using TaskManager.Domain.Account;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services
{
    public class UserService : BaseService<User, Guid, UserRequestDto, UserResponseDto>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticate _authenticate;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, IAuthenticate authenticate, ILogger<UserService> logger)
            : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _authenticate = authenticate;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResult<UserResponseDto>> RegisterUserAsync(UserRequestDto userRequestDto)
        {
            var existingUser = await _userRepository.GetFirstOrDefaultAsync(u => u.UserName == userRequestDto.UserName);
            if (existingUser != null)
            {
                return ServiceResult<UserResponseDto>.Fail("Username already taken.");
            }

            try
            {
                var hashedPassword = _authenticate.CryptoPassword(userRequestDto.Password);

                var user = new User(userRequestDto.UserName, hashedPassword);

                var createdUser = await _userRepository.AddAsync(user);

                var userResponseDto = _mapper.Map<UserResponseDto>(createdUser);

                return ServiceResult<UserResponseDto>.SuccessResult(userResponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");
                return ServiceResult<UserResponseDto>.Fail("An error occurred while registering the user. Please try again.");
            }
        }

        public async Task<ServiceResult<string>> LoginAsync(UserRequestDto userRequestDto)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(u => u.UserName == userRequestDto.UserName);
            if (user == null || !_authenticate.ValidatePassword(userRequestDto.Password, user.Password))
            {
                return ServiceResult<string>.Fail("Invalid username or password.");
            }

            try
            {
                var token = _authenticate.GenerateToken(user.Id, user.UserName);
                return ServiceResult<string>.SuccessResult(token);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return ServiceResult<string>.Fail("An error occurred while logging in. Please try again.");
            }
            
        }
    }
}
