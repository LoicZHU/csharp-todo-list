using Microsoft.EntityFrameworkCore;
using todo_list.DbContexts;
using todo_list.Entities;

namespace todo_list.Services.CategoryRepository;

public class CategoryRepository : TodoListRepository.TodoListRepository, ICategoryRepository
{
	public CategoryRepository(TodoListContext ctx)
		: base(ctx) { }

	public async Task<IEnumerable<Category>> GetCategories()
	{
		return await _todoListContext.Categories
			.AsNoTracking()
			.Include(category => category.Statistics)
			.ThenInclude(statistic => statistic.Todo)
			.ToListAsync();
	}
}
