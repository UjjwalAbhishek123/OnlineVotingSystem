using Microsoft.EntityFrameworkCore;
using OnlineVotingSystemAPI.Data;
using OnlineVotingSystemAPI.Repositories.Implementations;
using OnlineVotingSystemAPI.Repositories.Interfaces;
using OnlineVotingSystemAPI.Services.Implementations;
using OnlineVotingSystemAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Adding Repositpories and Services to IoC container
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

//adding ApplicationDbContext to Ioc Container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs"));
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
