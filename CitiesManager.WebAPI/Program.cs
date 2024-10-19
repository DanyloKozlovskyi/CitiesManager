using CitiesManager.DataAccess;
using CitiesManager.DataAccess.Identity;
using CitiesManager.WebAPI.Services;
using CitiesManager.WebAPI.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ICitiesService, CitiesService>();
builder.Services.AddTransient<IJwtService, JwtService>();

// Enable autoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Add DbContext
builder.Services.AddDbContext<CitiesDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Enable identity 
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<CitiesDbContext>()
.AddDefaultTokenProviders()
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, CitiesDbContext, Guid>>()
.AddRoleStore<RoleStore<ApplicationRole, CitiesDbContext, Guid>>();

//to use cors do that + app.UseCors()
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policyBuilder =>
	{
		policyBuilder
	 .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
	 .WithHeaders("Authorization", "origin", "accept", "content-type")
	 .WithMethods("GET", "POST", "PUT", "DELETE");
	});

	options.AddPolicy("3200Client", policyBuilder =>
	{
		policyBuilder
		.WithOrigins("http://localhost:3200")
		.WithHeaders("authorization", "origin", "accept", "content-type")
		.WithMethods("GET");
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts(); // enable https
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();
app.UseCors();

app.MapControllers();

app.Run();
