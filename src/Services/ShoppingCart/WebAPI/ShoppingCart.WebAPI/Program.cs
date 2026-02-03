using Discount.Grpc.Protos;
using MassTransit;
using ShoppingCart.Application;
using ShoppingCart.Infrastructure;
using ShoppingCart.WebAPI.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (x => x.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
builder.Services.AddScoped<DiscountGrpcService>();
//builder.Services.AddMassTransit(config => {
//                config.SetKebabCaseEndpointNameFormatter();
//                config.UsingRabbitMq((ctx, cfg) =>
//                    {
//                        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
//                        cfg.ConfigureEndpoints(ctx);
//                    });
//                });

//builder.Services.AddMassTransitHostedService();

builder.Services.AddControllers();
builder.Services.ConfigureShoppingCartApplicationServices(builder.Configuration);
builder.Services.ConfigureShoppingCartInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigration();
}

app.UseAuthorization();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
