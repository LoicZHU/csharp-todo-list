using todo_list.Entities;

namespace todo_list.Services.TodoRepository;

public interface ITodoRepository
{
	Task<Todo?> GetTodoById(Guid id);
	Task<IEnumerable<Todo>> GetTodos();
}
