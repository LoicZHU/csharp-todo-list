using todo_list.Entities;

namespace todo_list.Services.UserRepository;

public interface IUserRepository
{
	void AddUser(User user);
	void DeleteUser(Guid id);
	User GetUser(Guid id);
	IEnumerable<User> GetUsers();
	void UpdateUser(User user);
}
