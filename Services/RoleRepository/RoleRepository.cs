using Microsoft.EntityFrameworkCore;
using todo_list.DbContexts;
using todo_list.Entities;

namespace todo_list.Services.RoleRepository;

public class RoleRepository : TodoListRepository.TodoListRepository, IRoleRepository
{
	public RoleRepository(TodoListContext ctx)
		: base(ctx) { }

	public async Task AddRole(Role role)
	{
		await _todoListContext.Roles.AddAsync(role);
		await this.SaveAsync();
	}

	public async Task<Role?> GetRole(Guid id)
	{
		return await _todoListContext.Roles.FindAsync(id);
	}

	public async Task<IEnumerable<Role>> GetRoles()
	{
		return await _todoListContext.Roles.AsNoTracking().ToListAsync();
	}

	public async Task<bool> UpdateRole(Role role)
	{
		var roleToUpdate = await _todoListContext.Roles.FindAsync(role);

		if (roleToUpdate is null)
		{
			return false;
		}

		roleToUpdate = role;
		await this.SaveAsync();

		return true;
	}

	public async Task DeleteRole(Role role)
	{
		_todoListContext.Roles.Remove(role);
		await this.SaveAsync();
	}
}
