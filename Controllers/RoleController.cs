using Microsoft.AspNetCore.Mvc;
using todo_list.Entities;
using todo_list.Services.RoleRepository;

namespace todo_list.Controllers;

/// <summary>
/// TEST API CTRL
/// </summary>
[ApiController]
[Route("/api/roles")]
public class RoleController : Controller
{
	private readonly IRoleRepository _roleRepository;

	public RoleController(IRoleRepository roleRepository)
	{
		_roleRepository = roleRepository;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public CreatedResult AddRole([FromBody] Role role)
	{
		_roleRepository.AddRole(role);

		var newResourceUrl = Url.Action("GetRole", "Role", new { id = role.RoleId }, Request.Scheme);
		return Created(newResourceUrl, null);
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
