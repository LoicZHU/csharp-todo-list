using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.JsonWebTokens;
using todo_list.Entities;
using todo_list.Models;

namespace todo_list.Services.UserRepository;

public interface IUserRepository
{
	Task<bool> DeleteUser(User user);
	Task<User?> GetUser(User user);
	Task<User?> GetUserById(Guid id);
	Task<IEnumerable<User>> GetUsers();
	Task<bool> SignUp(User user);

	// JsonWebToken SignUp(UserDto userDto);
	Task<bool> UpdateUser(User user);
	Task<bool> UserExists(Guid id);
	Task<bool> UserExists(User user);
}
