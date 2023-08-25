using Microsoft.AspNetCore.Mvc;
using todo_list.Helpers;
using todo_list.Models;
using todo_list.Services.Auth;
using todo_list.Services.UserRepository;

namespace todo_list.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : ControllerBase
{
	private readonly AuthService _authService;
	private readonly IUserRepository _userRepository;

	public AuthController(AuthService authService, IUserRepository userRepository)
	{
		_authService = authService;
		_userRepository = userRepository;
	}

	[HttpPost]
	public async Task<IActionResult> LogIn([FromBody] LogInDto logInDto)
	{
		try
		{
			var user = await _userRepository.GetUserByEmail(logInDto.Email);
			if (user is null)
			{
				return NotFound();
			}

			if (!BCrypt.Net.BCrypt.Verify(logInDto.Password, user.Password))
			{
				return Unauthorized();
			}

			var token = _authService.GenerateJwt(user);
			return Ok(token);
		}
		catch (Exception e)
		{
			return this.HandleError(e.Message);
		}
	}
}
