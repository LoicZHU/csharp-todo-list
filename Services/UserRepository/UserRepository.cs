using System.ComponentModel.DataAnnotations;
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

	public async Task<User?> GetUserByEmail([EmailAddress] string email)
	{
		return await _todoListContext.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Email == email);
	}

	public async Task<IEnumerable<User>> GetUsers()
	{
		return await _todoListContext.Users.Include(user => user.Role).AsNoTracking().ToListAsync();
	}

	public async Task<bool> SignUp(User user)
	{
		await _todoListContext.Users.AddAsync(user);

		return await this.SaveAsync();
	}

	public async Task<bool> UpdateUser(User providedUser)
	{
		var userToUpdate = await _todoListContext.Users.FindAsync(providedUser.UserId);
		if (userToUpdate is null)
		{
			return false;
		}

		userToUpdate.FirstName = providedUser.FirstName;
		userToUpdate.Email = providedUser.Email;
		userToUpdate.Password = providedUser.Password;
		userToUpdate.RoleId = providedUser.RoleId;
		userToUpdate.UpdatedAt = DateTime.Now;

		return await this.SaveAsync();
	}

	public async Task<bool> UserExists([EmailAddress] string email)
	{
		return await _todoListContext.Users.AnyAsync(user => user.Email == email);
	}

	public async Task<bool> UserExists(User providedUser)
	{
		return await _todoListContext.Users.AnyAsync(
			user => user.UserId == providedUser.UserId || user.Email == providedUser.Email
		);
	}
}
