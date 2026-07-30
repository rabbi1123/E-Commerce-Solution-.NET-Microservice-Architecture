using MongoDB.Driver;
using Moq;
using ProductCatalog.WebAPI.Data;
using ProductCatalog.WebAPI.Entities;
using ProductCatalog.WebAPI.Repositories;

namespace ProductCatalog.WebAPI.UnitTest.Repositories;

public class ProductRepositoryTests
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

    private static (ProductRepository Repository, Mock<IMongoCollection<Product>> Collection)
        CreateRepository()
    {
        var mockCollection = new Mock<IMongoCollection<Product>>();
        var mockContext = new Mock<ICatalogContext>();
        mockContext.Setup(c => c.Products).Returns(mockCollection.Object);
        return (new ProductRepository(mockContext.Object), mockCollection);
    }

    private static Mock<IAsyncCursor<Product>> CreateCursor(IEnumerable<Product> products)
    {
        var mockCursor = new Mock<IAsyncCursor<Product>>();
        mockCursor.Setup(c => c.Current).Returns(products);
        mockCursor
            .SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        mockCursor
            .SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        return mockCursor;
    }

    [Fact]
    public void Constructor_WhenContextNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ProductRepository(null!));
    }

    [Fact]
    public async Task GetProducts_ReturnsProductsFromCollection()
    {
        var products = new List<Product>
        {
            SampleProduct("507f1f77bcf86cd799439011"),
            SampleProduct("507f1f77bcf86cd799439012")
        };
        var (repository, mockCollection) = CreateRepository();
        var mockCursor = CreateCursor(products);
        mockCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product, Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        var result = await repository.GetProducts();

        Assert.Equal(products, result);
        mockCollection.Verify(
            c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product, Product>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetProduct_WhenProductExists_ReturnsProduct()
    {
        var product = SampleProduct();
        var (repository, mockCollection) = CreateRepository();
        var mockCursor = CreateCursor(new[] { product });
        mockCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product, Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        var result = await repository.GetProduct(product.Id);

        Assert.Same(product, result);
    }

    [Fact]
    public async Task GetProductByName_ReturnsMatchingProducts()
    {
        var products = new List<Product> { SampleProduct() };
        var (repository, mockCollection) = CreateRepository();
        var mockCursor = CreateCursor(products);
        mockCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product, Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        var result = await repository.GetProductByName(products[0].Name);

        Assert.Equal(products, result);
    }

    [Fact]
    public async Task GetProductByCategory_ReturnsMatchingProducts()
    {
        var products = new List<Product> { SampleProduct() };
        var (repository, mockCollection) = CreateRepository();
        var mockCursor = CreateCursor(products);
        mockCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product, Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);

        var result = await repository.GetProductByCategory(products[0].Category);

        Assert.Equal(products, result);
    }

    [Fact]
    public async Task CreateProduct_CallsInsertOneAsync()
    {
        var product = SampleProduct();
        var (repository, mockCollection) = CreateRepository();
        mockCollection
            .Setup(c => c.InsertOneAsync(product, null, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await repository.CreateProduct(product);

        mockCollection.Verify(
            c => c.InsertOneAsync(product, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_WhenModified_ReturnsTrue()
    {
        var product = SampleProduct();
        var (repository, mockCollection) = CreateRepository();
        var replaceResult = new ReplaceOneResult.Acknowledged(1, 1, null);
        mockCollection
            .Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                product,
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(replaceResult);

        var result = await repository.UpdateProduct(product);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateProduct_WhenNotModified_ReturnsFalse()
    {
        var product = SampleProduct();
        var (repository, mockCollection) = CreateRepository();
        var replaceResult = new ReplaceOneResult.Acknowledged(1, 0, null);
        mockCollection
            .Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                product,
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(replaceResult);

        var result = await repository.UpdateProduct(product);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteProduct_WhenDeleted_ReturnsTrue()
    {
        const string id = "507f1f77bcf86cd799439011";
        var (repository, mockCollection) = CreateRepository();
        var deleteResult = new DeleteResult.Acknowledged(1);
        mockCollection
            .Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResult);

        var result = await repository.DeleteProduct(id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteProduct_WhenNotDeleted_ReturnsFalse()
    {
        const string id = "507f1f77bcf86cd799439099";
        var (repository, mockCollection) = CreateRepository();
        var deleteResult = new DeleteResult.Acknowledged(0);
        mockCollection
            .Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResult);

        var result = await repository.DeleteProduct(id);

        Assert.False(result);
    }
}
