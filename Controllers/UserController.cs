using AutoMapper;
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

	[HttpDelete]
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
	public async Task<IActionResult> SignUpUser([FromBody] UserDto userDto)
	{
		try
		{
			var mappedUser = _mapper.Map<User>(userDto);
			if (await _userRepository.UserExists(mappedUser))
			{
				return Conflict();
			}

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
}
