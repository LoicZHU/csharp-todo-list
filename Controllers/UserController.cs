using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_list.Entities;
using todo_list.Helpers;
using todo_list.Models;
using todo_list.Services.UserRepository;

namespace todo_list.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IUserRepository _userRepository;

	public UserController(IMapper mapper, IUserRepository userRepository)
	{
		_mapper = mapper;
		_userRepository = userRepository;
	}

	[HttpDelete("id/{id:guid}")]
	public async Task<IActionResult> DeleteUserById(Guid id)
	{
		try
		{
			var user = await _userRepository.GetUserById(id);
			if (user is null)
			{
				return NotFound();
			}

			var isDeleted = await _userRepository.DeleteUser(user);
			return !isDeleted ? this.HandleError("An error has occurred while deleting.") : NoContent();
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpDelete("email/{email}")]
	public async Task<IActionResult> DeleteUserByEmail([EmailAddress] string email)
	{
		try
		{
			// var mappedUser = _mapper.Map<User>(email);

			var user = await _userRepository.GetUserByEmail(email);
			if (user is null)
			{
				return NotFound();
			}

			var isDeleted = await _userRepository.DeleteUser(user);
			if (!isDeleted)
			{
				return this.HandleError("An error has occurred while deleting.");
			}

			return NoContent();
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpGet("{id}", Name = "GetUser")]
	[Authorize(Policy = Policy.AllowAdministrators)]
	public async Task<IActionResult> GetUser(Guid id)
	{
		try
		{
			var user = await _userRepository.GetUserById(id);
			if (user is null)
			{
				return NotFound();
			}

			return Ok(user);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpGet]
	[Authorize(Policy = Policy.AllowAdministrators)]
	public async Task<IActionResult> GetUsers()
	{
		try
		{
			var users = await _userRepository.GetUsers();

			return Ok(users);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpPost]
	[Route("sign-up")]
	[AllowAnonymous]
	public async Task<IActionResult> SignUpUser([FromBody] UserDto userDto)
	{
		try
		{
			var mappedUser = _mapper.Map<User>(userDto);
			if (await _userRepository.UserExists(mappedUser))
			{
				return Conflict();
			}

			mappedUser.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
			mappedUser.CreatedAt = DateTime.Now;
			var isAdded = await _userRepository.SignUp(mappedUser);
			if (!isAdded)
			{
				return this.HandleError("An error occurred while adding.");
			}

			var user = await _userRepository.GetUser(mappedUser);
			if (user is null)
			{
				return NotFound();
			}

			return CreatedAtRoute("GetUser", new { id = user.UserId }, null);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	/**
	 * var tokenHandler = new JwtSecurityTokenHandler();
	      var jwtToken = tokenHandler.ReadJwtToken(token);
	 */
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto userDto)
	{
		if (!HttpContext.Request.Headers.TryGetValue("Authorization", out var headerAuth))
		{
			return BadRequest();
		}

		var token = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
		var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

		var stringGuid = id.ToString();
		var tokenUserId = jwtToken.Claims
			.FirstOrDefault(c => string.Equals(c.Type, "nameId", StringComparison.OrdinalIgnoreCase))
			?.Value;
		var tokenUserRole = jwtToken.Claims
			.FirstOrDefault(c => string.Equals(c.Type, "role", StringComparison.OrdinalIgnoreCase))
			?.Value;

		var isSameUser = tokenUserId == stringGuid;
		var canUpdate =
			string.Equals(tokenUserRole, "Administrator", StringComparison.OrdinalIgnoreCase)
			|| !string.IsNullOrWhiteSpace(tokenUserId)
			|| !string.IsNullOrWhiteSpace(stringGuid)
			|| isSameUser;
		if (!canUpdate)
		{
			return BadRequest("Invalid ID.");
		}

		try
		{
			var user = await _userRepository.GetUserById(id);
			if (user is null)
			{
				return BadRequest("Invalid user.");
			}

			var mappedUser = _mapper.Map<User>(userDto);
			mappedUser.UserId = user.UserId;

			var isUpdated = await _userRepository.UpdateUser(mappedUser);
			return !isUpdated ? this.HandleError("An error has occurred while updating.") : Ok();
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
