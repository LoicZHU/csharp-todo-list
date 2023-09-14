using Microsoft.EntityFrameworkCore;
using todo_list.DbContexts;
using todo_list.Entities;

namespace todo_list.Services.RoleRepository;

public class RoleRepository : TodoListRepository.TodoListRepository, IRoleRepository
{
	public RoleRepository(TodoListContext ctx)
		: base(ctx) { }

	public async Task<bool> AddRole(Role role)
	{
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
		return await _todoListContext.Roles.AnyAsync(
			role => role.RoleId == providedRole.RoleId || role.Name == providedRole.Name
		);
	}

	public async Task<bool> RoleExists(Guid id)
	{
		return await _todoListContext.Roles.AnyAsync(role => role.RoleId == id);
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
