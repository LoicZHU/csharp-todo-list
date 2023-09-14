using Microsoft.EntityFrameworkCore;
using todo_list.DbContexts;
using todo_list.Entities;

namespace todo_list.Services.TodoRepository;

public class TodoRepository : TodoListRepository.TodoListRepository, ITodoRepository
{
	public TodoRepository(TodoListContext ctx)
		: base(ctx) { }

	public async Task<Todo?> GetTodoById(Guid id)
	{
		return await _todoListContext.Todos.FirstOrDefaultAsync(todo => todo.TodoId == id);
	}

	public async Task<IEnumerable<Todo>> GetTodos(int pageNumber)
	{
		var pageSize = 2;

		return await _todoListContext.Todos
			.AsTracking()
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.Include(todo => todo.Statistics)
			.ToListAsync();
	}
}
