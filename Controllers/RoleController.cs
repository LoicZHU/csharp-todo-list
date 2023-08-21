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
			await _roleRepository.AddRole(role);

			return CreatedAtRoute("GetRole", new { id = role.RoleId }, null);
		}
		catch (Exception e)
		{
			// Console.WriteLine(e);
			// return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
			return this.HandleError("An unexpected error occurred while processing the request.");
		}
	}

	[HttpGet("{id}", Name = "GetRole")]
	public async Task<ActionResult<Role?>> GetRole(Guid id)
	{
		var role = await _roleRepository.GetRole(id);
		if (role is null)
		{
			return NotFound();
		}

		return role;
	}

	[HttpGet]
	public async Task<IEnumerable<Role>> GetRoles()
	{
		return await _roleRepository.GetRoles();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteRole(Guid id)
	{
		var role = await _roleRepository.GetRole(id);
		if (role is null)
		{
			return NotFound();
		}

		await _roleRepository.DeleteRole(role);
		return NoContent();
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateRole(Guid id, [FromBody] Role updatedRole)
	{
		if (id != updatedRole.RoleId)
		{
			return BadRequest();
		}

		var isUpdated = await _roleRepository.UpdateRole(updatedRole);
		if (isUpdated == false)
		{
			return NotFound();
		}

		return NoContent();
	}
}
