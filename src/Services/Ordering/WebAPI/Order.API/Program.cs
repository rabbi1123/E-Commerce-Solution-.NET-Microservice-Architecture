using Order.API;
using Order.Application;
using Order.Infrastructure;
using MassTransit;
using Eventbus.Messages.Common;
using Order.API.EventBusConsumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOrderApplicationServices();
builder.Services.AddOrderInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(config => {
	config.SetKebabCaseEndpointNameFormatter();
	config.AddConsumer<CheckoutConsumer>();
	config.UsingRabbitMq((ctx, cfg) =>
	{
		cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
		cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
		{
			c.ConfigureConsumer<CheckoutConsumer>(ctx);
		});
		cfg.ConfigureEndpoints(ctx);
	});
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
