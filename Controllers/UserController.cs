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
			var user = _mapper.Map<User>(userDto);
			Console.WriteLine($"👉 ______________________________ {user.UserId}");

			if (await _userRepository.GetUserById(user.RoleId) is not null)
			{
				return BadRequest();
			}

			return Ok();
			// var isAdded = await _userRepository.AddRole(user);
			// if (!isAdded)
			// {
			//   return this.HandleError("An error occurred while adding.");
			// }
			//
			// return CreatedAtRoute("GetRole", new { id = user.UserId }, null);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
