using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using todo_list.DbContexts;
using todo_list.Services.RoleRepository;

namespace todo_list;

public class Startup
{
	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		this.Configuration = configuration;
	}

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		// Add services to the container.
		//services.AddDbContext...

		services
			.AddControllers(options =>
			{
				options.ReturnHttpNotAcceptable = true;
			})
			.AddNewtonsoftJson(options =>
			{
				// options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			});
		// .AddJsonOptions(options =>
		// {
		// 	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
		// });

		services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		services.AddScoped<IRoleRepository, RoleRepository>();

		services.AddDbContext<TodoListContext>(options =>
		{
			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
		});
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer().AddSwaggerGen();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		this.UseMiddlewares(app);
		this.AddEndpoints(app);
	}

	private void AddEndpoints(WebApplication app)
	{
		app.MapGet("/", () => "Hello World!");
		app.MapControllers();
	}

	private void UseMiddlewares(WebApplication app)
	{
		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
		else
		{
			app.UseExceptionHandler(appBuilder =>
			{
				appBuilder.Run(async httpCtx =>
				{
					httpCtx.Response.StatusCode = 500;
					await httpCtx.Response.WriteAsync("An unexpected fault happened. Try again later.");
				});
			});
		}

		app.UseHttpsRedirection();
		app.UseAuthorization();
	}
}
