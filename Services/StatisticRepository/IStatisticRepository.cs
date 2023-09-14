using todo_list.Entities;

namespace todo_list.Services.StatisticRepository;

public interface IStatisticRepository
{
	Task<IEnumerable<Statistic>> GetStatistics();
}
