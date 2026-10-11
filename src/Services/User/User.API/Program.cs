using EventBus.RabbitMQ;
using Eventbus.Messages.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.EventBusConsumer;
using User.API.Repositories;
using User.API.Services;

var builder = WebApplication.CreateBuilder(args);

var userConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__UserConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(userConnectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddRabbitMqEventBus(builder.Configuration, bus =>
{
    bus.AddConsumer<UserRegistrationConsumer>()
        .Endpoint(e => e.Name = EventBusConstants.UserRegistrationQueue);
});
builder.Services.AddScoped<UserRegistrationConsumer>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.ApplyMigration();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
