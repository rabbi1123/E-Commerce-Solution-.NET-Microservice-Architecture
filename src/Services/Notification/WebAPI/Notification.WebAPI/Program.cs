using EventBus.RabbitMQ;
using Eventbus.Messages.Common;
using MassTransit;
using Notification.Application;
using Notification.Infrastructure;
using Notification.WebAPI;
using Notification.WebAPI.EventBusConsumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNotificationApplication();
builder.Services.AddNotificationInfrastructure(builder.Configuration);
builder.Services.AddRabbitMqEventBus(builder.Configuration, bus =>
{
	bus.AddConsumer<UserVerificationEmailConsumer>()
		.Endpoint(e => e.Name = EventBusConstants.UserVerificationEmailQueue);
});
builder.Services.AddScoped<UserVerificationEmailConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.ApplyMigration();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
