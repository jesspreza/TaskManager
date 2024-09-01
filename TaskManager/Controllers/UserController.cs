using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Request;
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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(requestDto);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequestDto requestDto)
        {
            var result = await _userService.LoginAsync(requestDto);
            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new { Token = result.Data });
        }
    }
}
