using Microsoft.EntityFrameworkCore;
using todo_list.DbContexts;
using todo_list.Entities;
using todo_list.Helpers;

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

	public async Task<User?> GetUser(User providedUser)
	{
		return await _todoListContext.Users
			.Include(user => user.Role)
			.FirstOrDefaultAsync(user => user.UserId == providedUser.UserId || user.Email == providedUser.Email);
	}

	public async Task<User?> GetUserById(Guid id)
	{
		return await _todoListContext.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.UserId == id);
	}

	// public async Task<User?> GetUser(EmailAddressAttribute email)
	// {
	//    return await _todoListContext.Users.FindAsync(id);
	// }

	public async Task<IEnumerable<User>> GetUsers()
	{
		return await _todoListContext.Users.Include(user => user.Role).AsNoTracking().ToListAsync();
	}

	public async Task<bool> SignUp(User user)
	{
		user.Password = ShaPassword.SetPassword(user.Password);

		await _todoListContext.Users.AddAsync(user);
		return await this.SaveAsync();
	}

	// public  JsonWebToken SignUp(UserDto userDto)
	// {
	//    throw new NotImplementedException();
	// }

	public async Task<bool> UpdateUser(User user)
	{
		throw new NotImplementedException();
	}

	public Task<bool> UserExists(Guid id)
	{
		throw new NotImplementedException();
	}

	public async Task<bool> UserExists(User providedUser)
	{
		return await _todoListContext.Users.AnyAsync(
			user => user.UserId == providedUser.UserId || user.Email == providedUser.Email
		);
	}
}
