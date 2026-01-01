using Microsoft.AspNetCore.Mvc;
using MyHomeDigitalBookshelf.Application.Users;
using MyHomeDigitalBookshelf.Application.Users.Commands;
using MyHomeDigitalBookshelf.Application.Users.Queries;
using MyHomeDigitalBookshelf.Application.Common.Interfaces;

namespace MyHomeDigitalBookshelf.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ITokenService _tokenService;

    public AuthController(UserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserCommand command) // TODO: Create DTOs for commands and queries
    {
        try
        {
            var user = await _userService.CreateUserAsync(command);
            return Ok(new { UserId = user.Id });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticateUserQuery query) // TODO: Create DTOs for commands and queries
    {
        var user = await _userService.AuthenticateUserAsync(query);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var token = _tokenService.CreateToken(user);

        return Ok(new { Token = token });
    }
}
