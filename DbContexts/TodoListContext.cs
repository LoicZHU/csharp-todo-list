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

	private readonly Guid _adminGuid = Guid.NewGuid();
	private readonly Guid _userGuid = Guid.NewGuid();
	private readonly Guid _todoFeaturesGuid = Guid.NewGuid();
	private readonly Guid _todoCleanClothesGuid = Guid.NewGuid();
	private readonly Guid _todoCleanHouseGuid = Guid.NewGuid();

	private readonly Guid _categoryAppGuid = Guid.NewGuid();
	private readonly Guid _categoryHouseworkGuid = Guid.NewGuid();
	private readonly Guid _categoryHomeworkGuid = Guid.NewGuid();

	public TodoListContext(DbContextOptions<TodoListContext> options)
		: base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		this.AddDataOnRole(modelBuilder);
		this.AddDataOnUser(modelBuilder);
		this.AddDataOnTodo(modelBuilder);
		this.AddDataOnCategory(modelBuilder);
		this.AddDataOnStatistic(modelBuilder);

		this.ConfigureRelationships(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	private void ConfigureRelationships(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<User>()
			.HasOne(user => user.Role)
			.WithMany()
			.HasForeignKey(user => user.RoleId)
			.OnDelete(DeleteBehavior.SetNull);
	}

	private void AddDataOnCategory(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Category>()
			.HasData(
				new Category() { CategoryId = _categoryAppGuid, Name = "TodoList app" },
				new Category() { CategoryId = _categoryHouseworkGuid, Name = "Housework" },
				new Category() { CategoryId = _categoryHomeworkGuid, Name = "Homework" }
			);
	}

	private void AddDataOnRole(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Role>()
			.HasData(
				new Role() { RoleId = _adminGuid, Name = "Administrator" },
				new Role() { RoleId = _userGuid, Name = "User" }
			);
	}

	private void AddDataOnTodo(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Todo>()
			.HasData(
				new Todo()
				{
					TodoId = _todoFeaturesGuid,
					Title = "Implementing features",
					Description = "Improving app",
					Status = TodoStatus.Open,
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					UserId = _adminGuid
				},
				new Todo()
				{
					TodoId = _todoCleanClothesGuid,
					Title = "Clean clothes",
					Description = null,
					Status = TodoStatus.Open,
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					UserId = _userGuid
				},
				new Todo()
				{
					TodoId = _todoCleanHouseGuid,
					Title = "Clean house",
					Description = "Ground and windows",
					Status = TodoStatus.Open,
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					UserId = _userGuid
				}
			);
	}

	private void AddDataOnStatistic(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Statistic>()
			.HasData(
				new Statistic
				{
					StatisticId = Guid.NewGuid(),
					TodoId = _todoFeaturesGuid,
					CategoryId = _categoryAppGuid,
					CategoryTimesUsed = 1,
				},
				new Statistic
				{
					StatisticId = Guid.NewGuid(),
					TodoId = _todoCleanClothesGuid,
					CategoryId = _categoryHouseworkGuid,
					CategoryTimesUsed = 2,
				},
				new Statistic
				{
					StatisticId = Guid.NewGuid(),
					TodoId = _todoCleanHouseGuid,
					CategoryId = _categoryHouseworkGuid,
					CategoryTimesUsed = 2,
				}
			);
	}

	private void AddDataOnUser(ModelBuilder modelBuilder)
	{
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
					RoleId = _adminGuid
				},
				new User()
				{
					UserId = Guid.NewGuid(),
					FirstName = "Bob",
					Email = "user@todolist.fr",
					Password = BCrypt.Net.BCrypt.HashPassword(password),
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					RoleId = _userGuid
				}
			);
	}
}
