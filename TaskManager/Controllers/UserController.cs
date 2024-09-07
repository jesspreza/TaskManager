using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Request;
using TaskManager.Application.DTOs.Response;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Account;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, IAuthenticate authenticate)
        {
            _userService = userService;
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="requestDto">The details of the user to register.</param>
        /// <returns>A response indicating the result of the registration.</returns>
        /// <response code="200">Returns a success message with user registration details.</response>
        /// <response code="400">If the registration request is invalid or fails.</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(requestDto);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        /// <summary>
        /// Authenticate a user and generate a token.
        /// </summary>
        /// <param name="requestDto">The login credentials of the user.</param>
        /// <returns>A response with the authentication token if login is successful.</returns>
        /// <response code="200">Returns a token if authentication is successful.</response>
        /// <response code="401">If authentication fails due to invalid credentials.</response>
        /// <response code="400">If the login request is invalid or an error occurs.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserRequestDto requestDto)
        {
            try
            {
                var result = await _userService.LoginAsync(requestDto);
                if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }

                return Ok(new { Token = result.Data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
