using Microsoft.EntityFrameworkCore;
using todo_list;
using todo_list.DbContexts;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);

using (var scope = app.Services.CreateScope())
{
	try
	{
		var ctx = scope.ServiceProvider.GetService<TodoListContext>();
		// ctx.Database.EnsureDeleted();
		ctx.Database.EnsureCreated();
		// ctx.Database.Migrate();
	}
	catch (Exception e)
	{
		Console.WriteLine($"⚠️Erreur : {e}");
	}
}

app.Run();
