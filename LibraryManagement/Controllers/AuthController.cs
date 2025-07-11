using LibraryManagement.DTOs;
using LibraryManagement.Helpers;
using LibraryManagement.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);
            return ControllerHelper.HandleApiResponse(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);
            return ControllerHelper.HandleApiResponse(response);

        }

    }
}
