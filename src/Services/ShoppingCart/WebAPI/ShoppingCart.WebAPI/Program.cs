using Discount.Grpc.Protos;
using ShoppingCart.Application;
using ShoppingCart.Infrastructure;
using ShoppingCart.WebAPI;
using ShoppingCart.WebAPI.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var discountUrl = Environment.GetEnvironmentVariable("GrpcSettings__DiscountUrl");
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (x => x.Address = new Uri(discountUrl));
builder.Services.AddScoped<DiscountGrpcService>();

builder.Services.AddControllers();
builder.Services.ConfigureShoppingCartApplicationServices();
builder.Services.ConfigureShoppingCartInfrastructureServices();
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
