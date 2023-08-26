using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using todo_list.DbContexts;
using todo_list.Helpers;
using todo_list.Models;
using todo_list.Services.Auth;
using todo_list.Services.RoleRepository;
using todo_list.Services.UserRepository;

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
		this.ConfigureJwtAuth(services);

		this.AddAuthorization(services);

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

		this.AddScopedServices(services);

		this.AddDbContext(services);

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer().AddSwaggerGen();
	}

	private void AddAuthorization(IServiceCollection services)
	{
		services.AddAuthorization(options =>
		{
			options.AddPolicy(
				Policy.AllowAdministrators,
				policy =>
				{
					policy.RequireAuthenticatedUser().RequireRole("Administrator");
				}
			);
		});
	}

	private void AddDbContext(IServiceCollection services)
	{
		services.AddDbContext<TodoListContext>(options =>
		{
			options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
		});
	}

	private void AddEndpoints(WebApplication app)
	{
		app.MapGet(
			"/jwt/headers",
			(HttpContext ctx) =>
			{
				if (!ctx.Request.Headers.TryGetValue("Authorization", out var headerAuth))
				{
					return Task.FromResult((new { message = "jwt not found" }));
				}

				var jwtToken = headerAuth.First().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
				return Task.FromResult(jwtToken);
			}
		);

		app.MapGet("/", () => "Hello World!").RequireAuthorization();
		app.MapControllers().RequireAuthorization();
	}

	private void AddScopedServices(IServiceCollection services)
	{
		services
			.AddScoped<AuthService>()
			.AddScoped<IRoleRepository, RoleRepository>()
			.AddScoped<IUserRepository, UserRepository>();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(WebApplication app, IWebHostEnvironment env)
	{
		this.UseMiddlewares(app);
		this.AddEndpoints(app);
	}

	private void ConfigureJwtAuth(IServiceCollection services)
	{
		var jwtSettingsConfigurationSection = Configuration.GetSection("JwtSettings");
		var jwtSettings = jwtSettingsConfigurationSection.Get<JwtSettings>();
		services.Configure<JwtSettings>(jwtSettingsConfigurationSection);
		services.AddSingleton(jwtSettings);

		services
			.AddAuthentication(options =>
			{
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
				};
			});
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
		app.UseAuthentication();
		app.UseAuthorization();
	}
}
