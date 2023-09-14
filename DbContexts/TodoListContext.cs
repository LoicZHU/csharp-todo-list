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
	private readonly Guid _todoCleanPet = Guid.NewGuid();

	private readonly Guid _categoryAppGuid = Guid.NewGuid();
	private readonly Guid _categoryHouseworkGuid = Guid.NewGuid();
	private readonly Guid _categoryHomeworkGuid = Guid.NewGuid();
	private readonly Guid _categoryWeeklyHouseworkGuid = Guid.NewGuid();

	public TodoListContext(DbContextOptions<TodoListContext> options)
		: base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSnakeCaseNamingConvention();
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		this.ConfigureRelationships(modelBuilder);
		this.AddDataOnTables(modelBuilder);

		base.OnModelCreating(modelBuilder);
	}

	private void AddDataOnTables(ModelBuilder modelBuilder)
	{
		this.AddDataOnRole(modelBuilder);
		this.AddDataOnUser(modelBuilder);
		this.AddDataOnTodo(modelBuilder);
		this.AddDataOnCategory(modelBuilder);
		this.AddDataOnStatistic(modelBuilder);
	}

	private void ConfigureRelationships(ModelBuilder modelBuilder)
	{
		this.ConfigureRelationshipBetweenUserAndRole(modelBuilder);
		this.ConfigureRelationshipBetweenTodoAndUser(modelBuilder);
		this.ConfigureRelationshipStatisticBetweenTodoAndCategory(modelBuilder);
	}

	private void ConfigureRelationshipBetweenUserAndRole(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<User>()
			.HasOne(user => user.Role)
			.WithMany()
			.HasForeignKey(user => user.RoleId)
			.OnDelete(DeleteBehavior.SetNull);
	}

	private void ConfigureRelationshipBetweenTodoAndUser(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Todo>()
			.HasOne(todo => todo.User)
			.WithMany()
			.HasForeignKey(todo => todo.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}

	private void ConfigureRelationshipStatisticBetweenTodoAndCategory(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Statistic>().HasKey(statistic => new { statistic.TodoId, statistic.CategoryId });

		modelBuilder
			.Entity<Statistic>()
			.HasOne(statistic => statistic.Todo)
			.WithMany(todo => todo.Statistics)
			.HasForeignKey(statistic => statistic.TodoId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder
			.Entity<Statistic>()
			.HasOne(statistic => statistic.Category)
			.WithMany(category => category.Statistics)
			.HasForeignKey(statistic => statistic.CategoryId)
			.OnDelete(DeleteBehavior.Cascade);

		// modelBuilder
		// 	.Entity<Todo>()
		// 	.HasMany(todo => todo.Categories)
		// 	.WithMany(category => category.Todos)
		// 	.UsingEntity<Statistic>(
		// 		// builder => builder.HasOne<Category>().WithMany().HasForeignKey(statistic => statistic.CategoryId),
		// 		// builder => builder.HasOne<Todo>().WithMany().HasForeignKey(statistic => statistic.TodoId)
		// 	);
	}

	private void AddDataOnCategory(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Category>()
			.HasData(
				new Category() { CategoryId = _categoryAppGuid, Name = "TodoList app" },
				new Category() { CategoryId = _categoryHouseworkGuid, Name = "Housework" },
				new Category() { CategoryId = _categoryWeeklyHouseworkGuid, Name = "Weekly housework" },
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
					Description = "Floor and windows",
					Status = TodoStatus.Open,
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					UserId = _userGuid,
				},
				new Todo()
				{
					TodoId = _todoCleanPet,
					Title = "Clean pet",
					Description = "Make it take a bath",
					Status = TodoStatus.Open,
					CreatedAt = DateTime.Now.AddMinutes(5),
					UpdatedAt = null,
					UserId = _userGuid,
				}
			);
	}

	private void AddDataOnStatistic(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Statistic>()
			.HasData(
				new Statistic { TodoId = _todoFeaturesGuid, CategoryId = _categoryAppGuid, },
				new Statistic { TodoId = _todoCleanClothesGuid, CategoryId = _categoryHouseworkGuid, },
				new Statistic { TodoId = _todoCleanClothesGuid, CategoryId = _categoryWeeklyHouseworkGuid, },
				new Statistic { TodoId = _todoCleanHouseGuid, CategoryId = _categoryHouseworkGuid, },
				new Statistic { TodoId = _todoCleanPet, CategoryId = _categoryHomeworkGuid, },
				new Statistic { TodoId = _todoCleanPet, CategoryId = _categoryWeeklyHouseworkGuid, }
			);
	}

	private void AddDataOnUser(ModelBuilder modelBuilder)
	{
		const string password = "123456789";

		modelBuilder
			.Entity<User>()
			.HasData(
				new User()
				{
					UserId = _adminGuid,
					FirstName = "Alex",
					Email = "admin@todolist.fr",
					Password = BCrypt.Net.BCrypt.HashPassword(password),
					CreatedAt = DateTime.Now,
					UpdatedAt = null,
					RoleId = _adminGuid
				},
				new User()
				{
					UserId = _userGuid,
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
