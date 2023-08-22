using todo_list.Entities;
using todo_list.Models;

namespace todo_list.Services.RoleRepository;

public interface IRoleRepository
{
	Task AddRole(Role role);
	Task<bool> DeleteRole(Role role);
	Task<Role?> GetRole(Guid id);
	Task<IEnumerable<Role>> GetRoles();
	Task<bool> UpdateRole(Role role);
}
