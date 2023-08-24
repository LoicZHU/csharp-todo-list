using todo_list.Entities;
using todo_list.Models;

namespace todo_list.Services.RoleRepository;

public interface IRoleRepository
{
	Task<bool> AddRole(RoleDto roleDto);
	Task<bool> DeleteRole(Role role);
	Task<Role?> GetRole(Role role);
	Task<Role?> GetRole(RoleDto roleDto);
	Task<Role?> GetRoleById(Guid roleId);
	Task<IEnumerable<Role>> GetRoles();
	Task<bool> RoleExists(Guid id);
	Task<bool> RoleExists(Role role);
	Task<bool> RoleExists(RoleDto roleDto);
	Task<bool> UpdateRole(Role role);
}
