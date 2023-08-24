using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using todo_list.DbContexts;
using todo_list.Entities;

namespace todo_list.Services.UserRepository;

public class UserRepository : TodoListRepository.TodoListRepository, IUserRepository
{
	public UserRepository(TodoListContext ctx)
		: base(ctx) { }

	public async Task<bool> DeleteUser(User user)
	{
		_todoListContext.Users.Remove(user);

		return await this.SaveAsync();
	}

	public async Task<User?> GetUserById(Guid id)
	{
		return await _todoListContext.Users.FindAsync(id);
	}

	// public async Task<User?> GetUser(EmailAddressAttribute email)
	// {
	//    return await _todoListContext.Users.FindAsync(id);
	// }

	public async Task<IEnumerable<User>> GetUsers()
	{
		return await _todoListContext.Users.AsNoTracking().ToListAsync();
		// return await _todoListContext.Users.ToListAsync();
	}

	public JsonWebToken SignUp()
	{
		throw new NotImplementedException();
	}

	public async Task<bool> UpdateUser(User user)
	{
		throw new NotImplementedException();
	}
}
