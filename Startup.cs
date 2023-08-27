using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using todo_list.DbContexts;
using todo_list.Helpers;
using todo_list.Models;
using todo_list.Services.Auth;
using todo_list.Services.RoleRepository;
using todo_list.Services.UserRepository;
using OpenApiContact = Microsoft.OpenApi.Models.OpenApiContact;
using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;
using OpenApiLicense = Microsoft.OpenApi.Models.OpenApiLicense;

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

		this.AddOpenApiDocument(services);

		services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		this.AddScopedServices(services);

		this.AddDbContext(services);

		this.ConfigureSwaggerFromSwashbuckle(services);
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

	private void AddOpenApiDocument(IServiceCollection services)
	{
		services.AddOpenApiDocument(settings =>
		{
			settings.PostProcess = document =>
			{
				document.Info = new NSwag.OpenApiInfo()
				{
					Version = "v1",
					Title = "TodoList API",
					Description = "An ASP.NET Core web API for managing todo items!",
					TermsOfService = "https://example.com/terms",
					Contact = new NSwag.OpenApiContact() { Name = "Example Contact", Url = "https://example.com/contact" },
					License = new NSwag.OpenApiLicense() { Name = "Example License", Url = "https://example.com/license" }
				};
			};

			settings.AddSecurity(
				"Bearer",
				Enumerable.Empty<string>(),
				new NSwag.OpenApiSecurityScheme()
				{
					Type = OpenApiSecuritySchemeType.Http,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					BearerFormat = "JWT",
					Description = "Type into the textbox: {your JWT token}."
				}
			);

			settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
		});
	}

	private void ConfigureSwaggerFromSwashbuckle(IServiceCollection services)
	{
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services
			.AddEndpointsApiExplorer()
			.AddSwaggerGen(options =>
			{
				options.SwaggerDoc(
					"v1",
					new OpenApiInfo
					{
						Version = "v1",
						Title = "TodoList API",
						Description = "An ASP.NET Core web API for managing todo items!",
						TermsOfService = new Uri("https://example.com/terms"),
						Contact = new OpenApiContact { Name = "Example Contact", Url = new Uri("https://example.com/contact") },
						License = new OpenApiLicense { Name = "Example License", Url = new Uri("https://example.com/license") }
					}
				);

				var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
			});
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
			// this.UseSwaggerFromSwashbuckle(app);
			this.UseSwaggerFromNswag(app);
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

	private void UseSwaggerFromNswag(WebApplication app)
	{
		// Add OpenAPI 3.0 document serving middleware
		// Available at: http://localhost:<port>/swagger/v1/swagger.json
		app.UseOpenApi();

		// Add web UIs to interact with the document
		// Available at: http://localhost:<port>/swagger
		app.UseSwaggerUi3(settings =>
		{
			settings.DocExpansion = "list";
		});

		// Add ReDoc UI to interact with the document
		// Available at: http://localhost:<port>/redoc
		app.UseReDoc(options =>
		{
			options.Path = "/redoc";
		});
	}

	private void UseSwaggerFromSwashbuckle(WebApplication app)
	{
		// app.UseSwagger();
		app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			options.RoutePrefix = string.Empty;
		});
	}
}
