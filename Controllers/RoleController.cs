using Microsoft.AspNetCore.Mvc;
using todo_list.Entities;
using todo_list.Helpers;
using todo_list.Models;
using todo_list.Services.RoleRepository;

namespace todo_list.Controllers;

[ApiController]
[Route("/api/roles")]
public class RoleController : ControllerBase
{
	private readonly IRoleRepository _roleRepository;

	public RoleController(IRoleRepository roleRepository)
	{
		_roleRepository = roleRepository;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto)
	{
		try
		{
			if (await _roleRepository.RoleExists(roleDto))
			{
				return Conflict();
			}

			var isAdded = await _roleRepository.AddRole(roleDto);
			var role = await _roleRepository.GetRole(roleDto);

			return !isAdded
				? this.HandleError("An error occurred while adding.")
				: CreatedAtRoute("GetRole", new { id = role.RoleId }, null);
		}
		catch (Exception e)
		{
			return this.HandleError(e.Message);
		}
	}

	[HttpGet("{id}", Name = "GetRole")]
	[ProducesResponseType(typeof(Role), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetRole(Guid id)
	{
		try
		{
			var role = await _roleRepository.GetRoleById(id);
			if (role is null)
			{
				return NotFound();
			}

			return Ok(role);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpGet]
	public async Task<IActionResult> GetRoles()
	{
		try
		{
			var roles = await _roleRepository.GetRoles();

			return Ok(roles);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteRole(Guid id)
	{
		try
		{
			if (!await _roleRepository.RoleExists(id))
			{
				return NotFound();
			}

			var role = await _roleRepository.GetRoleById(id);

			var isDeleted = await _roleRepository.DeleteRole(role);
			return !isDeleted ? this.HandleError("An error has occurred while deleting.") : NoContent();
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateRole(Guid id, [FromBody] Role updatedRole)
	{
		try
		{
			if (id != updatedRole.RoleId)
			{
				return BadRequest();
			}

			var isInvalidRole = updatedRole.Name != RoleName.Administrator && updatedRole.Name != RoleName.User;
			if (isInvalidRole)
			{
				return BadRequest();
			}

			var role = await _roleRepository.GetRoleById(id);
			if (role is null)
			{
				return NotFound();
			}

			var isUpdated = await _roleRepository.UpdateRole(updatedRole);
			if (!isUpdated)
			{
				return BadRequest();
			}

			return NoContent();
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
