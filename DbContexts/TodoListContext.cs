using Microsoft.EntityFrameworkCore;
using todo_list.Entities;

namespace todo_list.DbContexts;

public class TodoListContext : DbContext
{
	public DbSet<Role> Roles { get; set; }
	public DbSet<Avatar> Avatars { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Todo> Todos { get; set; }
	public DbSet<Statistic> Statistics { get; set; }
	public DbSet<Category> Categories { get; set; }

	public TodoListContext(DbContextOptions<TodoListContext> options)
		: base(options) { }
}
