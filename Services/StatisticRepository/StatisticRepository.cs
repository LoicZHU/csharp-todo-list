using Microsoft.EntityFrameworkCore;
using todo_list.DbContexts;
using todo_list.Entities;

namespace todo_list.Services.StatisticRepository;

public class StatisticRepository : TodoListRepository.TodoListRepository, IStatisticRepository
{
	public StatisticRepository(TodoListContext ctx)
		: base(ctx) { }

	public async Task<IEnumerable<Statistic>> GetStatistics()
	{
		return await _todoListContext.Statistics
			.AsNoTracking()
			.Include(statistic => statistic.Category)
			.Include(statistic => statistic.Todo)
			.ThenInclude(todo => todo.User)
			.ToListAsync();
	}
}
