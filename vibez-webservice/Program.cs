using Microsoft.EntityFrameworkCore;
using SOA_CA2.Infrastructure;
using SOA_CA2;
using SOA_CA2.Interfaces;
using SOA_CA2.Middleware;
using SOA_CA2.Repositories;
using SOA_CA2.Services;
using SOA_CA2.Utilities;
using SOA_CA2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DBConn"))
);

// Add AutoMapper
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add Scoped Dependencies
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IOtpCacheManager, OtpCacheManager>(); // Singleton for caching OTPs

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Add custom middleware for JWT and Error Handling
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Enable static files for the profile picture uploads
app.UseStaticFiles();

app.Run();
