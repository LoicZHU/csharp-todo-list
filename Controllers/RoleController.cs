using System.Net;
using AutoMapper;
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
		try
		{
			var role = _mapper.Map<Role>(roleDto);
			var isAdded = await _roleRepository.AddRole(role);
			if (!isAdded)
			{
				return this.HandleError("An error occurred while adding.");
			}

			return CreatedAtRoute("GetRole", new { id = role.RoleId }, null);
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}

	[HttpGet("{id}", Name = "GetRole")]
	public async Task<IActionResult> GetRole(Guid id)
	{
		try
		{
			var role = await _roleRepository.GetRole(id);
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
			var role = await _roleRepository.GetRole(id);
			if (role is null)
			{
				return NotFound();
			}

			var isDeleted = await _roleRepository.DeleteRole(role);
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

			var role = await _roleRepository.GetRole(id);
			if (role is null)
			{
				return NotFound();
			}

			var isUpdated = await _roleRepository.UpdateRole(updatedRole);
			if (!isUpdated)
			{
				return this.HandleError("An error has occurred while updating.");
			}

			return NoContent();
		}
		catch (Exception e)
		{
			return this.HandleError();
		}
	}
}
