using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.JsonWebTokens;
using todo_list.Entities;

namespace todo_list.Services.UserRepository;

public interface IUserRepository
{
	Task<bool> DeleteUser(User user);
	Task<User?> GetUserById(Guid id);

	// Task<User?> GetUserByEmail(EmailAddressAttribute email);
	Task<IEnumerable<User>> GetUsers();
	JsonWebToken SignUp();
	Task<bool> UpdateUser(User user);
}
