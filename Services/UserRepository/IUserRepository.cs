using System.ComponentModel.DataAnnotations;
using todo_list.Entities;

namespace todo_list.Services.UserRepository;

public interface IUserRepository
{
	Task<bool> DeleteUser(User user);
	Task<User?> GetUser(User user);
	Task<User?> GetUserById(Guid id);
	Task<User?> GetUserByEmail([EmailAddress] string email);
	Task<IEnumerable<User>> GetUsers();
	Task<bool> SignUp(User user);

	Task<bool> UpdateUser(User user);
	Task<bool> UserExists([EmailAddress] string email);
	Task<bool> UserExists(User user);
}
