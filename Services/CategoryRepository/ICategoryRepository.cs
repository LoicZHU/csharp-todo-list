using todo_list.Entities;

namespace todo_list.Services.CategoryRepository;

public interface ICategoryRepository
{
	Task<IEnumerable<Category>> GetCategories();
}
