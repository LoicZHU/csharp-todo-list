using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo_list.Entities;
using todo_list.Helpers;
using todo_list.Models;
using todo_list.Services.RoleRepository;

namespace todo_list.Controllers;

[ApiController]
[Route("/api/roles")]
[Authorize(Policy = Policy.AllowAdministrators)]
public class RoleController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IRoleRepository _roleRepository;

	public RoleController(IMapper mapper, IRoleRepository roleRepository)
	{
		_mapper = mapper;
		_roleRepository = roleRepository;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> AddRole([FromBody] RoleDto roleDto)
	{
		if (string.IsNullOrWhiteSpace(roleDto.Name))
		{
			return BadRequest();
		}

		try
		{
			var mappedRole = _mapper.Map<Role>(roleDto);
			var roleExists = await _roleRepository.RoleExists(mappedRole);
			if (roleExists)
			{
				return Conflict();
			}

			var isAdded = await _roleRepository.AddRole(mappedRole);
			if (!isAdded)
			{
				return this.HandleError("An error occurred while adding.");
			}

			var role = await _roleRepository.GetRole(mappedRole);
			if (role is null)
			{
				return NotFound();
			}

			return !isAdded
				? this.HandleError("An error occurred while adding.")
				: CreatedAtRoute("GetRole", new { id = role.RoleId }, null);
		}
		catch (Exception e)
		{
			return this.HandleError(e.InnerException.Message);
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
		if (id != updatedRole.RoleId)
		{
			return BadRequest();
		}

		if (string.IsNullOrWhiteSpace(updatedRole.Name))
		{
			return BadRequest();
		}

		try
		{
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
