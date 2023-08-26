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

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var adminGuid = Guid.NewGuid();
		var userGuid = Guid.NewGuid();
		modelBuilder
			.Entity<Role>()
			.HasData(
				new Role() { RoleId = adminGuid, Name = "Administrator" },
				new Role() { RoleId = userGuid, Name = "User" }
			);

		var password = "123456789";
		modelBuilder
			.Entity<User>()
			.HasData(
				new User()
				{
					UserId = Guid.NewGuid(),
					FirstName = "Alex",
					Email = "admin@todolist.fr",
					Password = BCrypt.Net.BCrypt.HashPassword(password),
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					RoleId = adminGuid
				},
				new User()
				{
					UserId = Guid.NewGuid(),
					FirstName = "Bob",
					Email = "user@todolist.fr",
					Password = BCrypt.Net.BCrypt.HashPassword(password),
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					RoleId = userGuid
				}
			);

		base.OnModelCreating(modelBuilder);
	}
}
