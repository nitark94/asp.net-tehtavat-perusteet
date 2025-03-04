using Microsoft.AspNetCore.Mvc;
using Authorisation_web_api.Models;
using Authorisation_web_api.Services;

namespace Authorisation_web_api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (_userService.Register(user.Username, user.PasswordHash))
            return Ok(new { message = "User registered successfully" });

        return BadRequest(new { message = "User already exists" });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        if (_userService.ValidateUser(user.Username, user.PasswordHash))
        {
            return Ok(new { message = "Login successful" });
        }

        return Unauthorized(new { message = "Invalid username or password" });
    }
}
