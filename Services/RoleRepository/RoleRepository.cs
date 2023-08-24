using AutoMapper;
using Microsoft.EntityFrameworkCore;
using todo_list.DbContexts;
using todo_list.Entities;
using todo_list.Models;

namespace todo_list.Services.RoleRepository;

public class RoleRepository : TodoListRepository.TodoListRepository, IRoleRepository
{
	protected readonly IMapper _mapper;

	public RoleRepository(IMapper mapper, TodoListContext ctx)
		: base(ctx)
	{
		_mapper = mapper;
	}

	public async Task<bool> AddRole(RoleDto roleDto)
	{
		var role = _mapper.Map<Role>(roleDto);

		await _todoListContext.Roles.AddAsync(role);
		return await this.SaveAsync();
	}

	public async Task<bool> DeleteRole(Role role)
	{
		_todoListContext.Roles.Remove(role);

		return await this.SaveAsync();
	}

	public async Task<Role?> GetRole(Role providedRole)
	{
		return await _todoListContext.Roles.FirstOrDefaultAsync(
			role => role.RoleId == providedRole.RoleId || role.Name == providedRole.Name
		);
	}

	public async Task<Role?> GetRole(RoleDto roleDto)
	{
		return await _todoListContext.Roles.FirstOrDefaultAsync(role => role.Name == roleDto.Name);
	}

	public async Task<Role?> GetRoleById(Guid roleId)
	{
		return await _todoListContext.Roles.FindAsync(roleId);
	}

	public async Task<IEnumerable<Role>> GetRoles()
	{
		return await _todoListContext.Roles.AsNoTracking().ToListAsync();
	}

	public async Task<bool> RoleExists(Role providedRole)
	{
		return await _todoListContext.Roles.AnyAsync(role => role.RoleId == providedRole.RoleId);
	}

	public async Task<bool> RoleExists(Guid id)
	{
		return await _todoListContext.Roles.AnyAsync(role => role.RoleId == id);
	}

	public async Task<bool> RoleExists(RoleDto roleDto)
	{
		return await _todoListContext.Roles.AnyAsync(role => role.Name == roleDto.Name);
	}

	public async Task<bool> UpdateRole(Role updatedRole)
	{
		var roleToUpdate = await _todoListContext.Roles.FindAsync(updatedRole.RoleId);
		if (roleToUpdate is null)
		{
			return false;
		}

		roleToUpdate.Name = updatedRole.Name;

		return await this.SaveAsync();
	}
}
