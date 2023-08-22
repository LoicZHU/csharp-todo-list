using todo_list.DbContexts;

namespace todo_list.Services.TodoListRepository;

public abstract class TodoListRepository : ITodoListRepository
{
	protected readonly TodoListContext _todoListContext;

	protected TodoListRepository(TodoListContext ctx)
	{
		_todoListContext = ctx;
	}

	public async Task<bool> SaveAsync()
	{
		return await _todoListContext.SaveChangesAsync() >= 1;
	}
}
