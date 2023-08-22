namespace todo_list.Services.TodoListRepository;

public interface ITodoListRepository
{
	Task<bool> SaveAsync();
}
