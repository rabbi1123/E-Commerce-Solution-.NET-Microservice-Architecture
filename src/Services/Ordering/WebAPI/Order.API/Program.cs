using EventBus.RabbitMQ;
using Eventbus.Messages.Common;
using MassTransit;
using Order.API;
using Order.API.EventBusConsumer;
using Order.Application;
using Order.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOrderApplicationServices();
builder.Services.AddOrderInfrastructureServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRabbitMqEventBus(builder.Configuration, bus =>
{
	bus.AddConsumer<CheckoutConsumer>()
		.Endpoint(e => e.Name = EventBusConstants.BasketCheckoutQueue);
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<CheckoutConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
