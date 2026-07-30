using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.WebAPI.Controllers;
using ProductCatalog.WebAPI.Entities;
using ProductCatalog.WebAPI.Repositories;

namespace ProductCatalog.WebAPI.UnitTest.Controllers;

public class CatalogControllerTests
{
    private static Product SampleProduct(string? id = null) => new()
    {
        Id = id ?? "507f1f77bcf86cd799439011",
        Name = "Test Product",
        Category = "Electronics",
        Summary = "Summary",
        Description = "Description",
        ImageFile = "image.jpg",
        Price = 99.99m
    };

    private static CatalogController CreateController(
        Mock<IProductRepository> mockRepository,
        Mock<ILogger<CatalogController>>? mockLogger = null)
    {
        return new CatalogController(
            mockRepository.Object,
            (mockLogger ?? new Mock<ILogger<CatalogController>>()).Object);
    }

    [Fact]
    public async Task GetProducts_CallsRepositoryAndReturnsOkWithProducts()
    {
        var products = new List<Product>
        {
            SampleProduct("507f1f77bcf86cd799439011"),
            SampleProduct("507f1f77bcf86cd799439012")
        };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.GetProducts()).ReturnsAsync(products);

        var controller = CreateController(mockRepository);

        var result = await controller.GetProducts();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(products, ok.Value);
        mockRepository.Verify(r => r.GetProducts(), Times.Once);
    }

    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsOkWithProduct()
    {
        var product = SampleProduct();
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.GetProduct(product.Id)).ReturnsAsync(product);

        var controller = CreateController(mockRepository);

        var result = await controller.GetProductById(product.Id);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(product, ok.Value);
        mockRepository.Verify(r => r.GetProduct(product.Id), Times.Once);
    }

    [Fact]
    public async Task GetProductById_WhenProductMissing_ReturnsNotFound()
    {
        const string id = "507f1f77bcf86cd799439099";
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.GetProduct(id)).ReturnsAsync((Product)null!);
        var mockLogger = new Mock<ILogger<CatalogController>>();

        var controller = CreateController(mockRepository, mockLogger);

        var result = await controller.GetProductById(id);

        Assert.IsType<NotFoundResult>(result.Result);
        mockRepository.Verify(r => r.GetProduct(id), Times.Once);
    }

    [Fact]
    public async Task GetProductByName_CallsRepositoryAndReturnsOkWithProducts()
    {
        const string name = "Test Product";
        var products = new List<Product> { SampleProduct() };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.GetProductByName(name)).ReturnsAsync(products);

        var controller = CreateController(mockRepository);

        var result = await controller.GetProductByName(name);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(products, ok.Value);
        mockRepository.Verify(r => r.GetProductByName(name), Times.Once);
    }

    [Fact]
    public async Task GetProductByCategory_CallsRepositoryAndReturnsOkWithProducts()
    {
        const string category = "Electronics";
        var products = new List<Product> { SampleProduct() };
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.GetProductByCategory(category)).ReturnsAsync(products);

        var controller = CreateController(mockRepository);

        var result = await controller.GetProductByCategory(category);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(products, ok.Value);
        mockRepository.Verify(r => r.GetProductByCategory(category), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_CallsRepositoryAndReturnsCreatedAtRoute()
    {
        var product = SampleProduct();
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.CreateProduct(product)).Returns(Task.CompletedTask);

        var controller = CreateController(mockRepository);

        var result = await controller.CreateProduct(product);

        var created = Assert.IsType<CreatedAtRouteResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        Assert.Equal("GetProduct", created.RouteName);
        Assert.True(created.RouteValues!.TryGetValue("id", out var routeId));
        Assert.Equal(product.Id, Assert.IsType<string>(routeId));
        Assert.Same(product, created.Value);
        mockRepository.Verify(r => r.CreateProduct(product), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_CallsRepositoryAndReturnsOkWithResult()
    {
        var product = SampleProduct();
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.UpdateProduct(product)).ReturnsAsync(true);

        var controller = CreateController(mockRepository);

        var result = await controller.UpdateProduct(product);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)(ok.Value ?? false));
        mockRepository.Verify(r => r.UpdateProduct(product), Times.Once);
    }

    [Fact]
    public async Task DeleteProductById_CallsRepositoryAndReturnsOkWithResult()
    {
        const string id = "507f1f77bcf86cd799439011";
        var mockRepository = new Mock<IProductRepository>();
        mockRepository.Setup(r => r.DeleteProduct(id)).ReturnsAsync(true);

        var controller = CreateController(mockRepository);

        var result = await controller.DeleteProductById(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)(ok.Value ?? false));
        mockRepository.Verify(r => r.DeleteProduct(id), Times.Once);
    }

    [Fact]
    public void Constructor_WhenRepositoryNull_ThrowsArgumentNullException()
    {
        var logger = new Mock<ILogger<CatalogController>>();

        Assert.Throws<ArgumentNullException>(() => new CatalogController(null!, logger.Object));
    }

    [Fact]
    public void Constructor_WhenLoggerNull_ThrowsArgumentNullException()
    {
        var repository = new Mock<IProductRepository>();

        Assert.Throws<ArgumentNullException>(() => new CatalogController(repository.Object, null!));
    }
}
