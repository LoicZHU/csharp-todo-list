using todo_list.Entities;

namespace todo_list.Services.RoleRepository;

public interface IRoleRepository
{
	Task AddRole(Role role);
	Task DeleteRole(Role role);
	Task<Role?> GetRole(Guid id);
	Task<IEnumerable<Role>> GetRoles();
	Task<bool> UpdateRole(Role role);
}
