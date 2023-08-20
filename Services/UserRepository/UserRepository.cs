using todo_list.DbContexts;
using todo_list.Entities;

namespace todo_list.Services.UserRepository;

public class UserRepository : IUserRepository
{
	private readonly TodoListContext _todoListContext;

	public UserRepository(TodoListContext ctx)
	{
		_todoListContext = ctx;
	}

	public void AddUser(User user)
	{
		throw new NotImplementedException();
	}

	public User GetUser(Guid id)
	{
		throw new NotImplementedException();
	}

	public IEnumerable<User> GetUsers()
	{
		return _todoListContext.Users;
	}

	public void UpdateUser(User user)
	{
		throw new NotImplementedException();
	}

	public void DeleteUser(Guid id)
	{
		throw new NotImplementedException();
	}
}
