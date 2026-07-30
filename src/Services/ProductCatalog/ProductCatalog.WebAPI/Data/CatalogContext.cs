using MongoDB.Driver;
using ProductCatalog.WebAPI.Entities;

namespace ProductCatalog.WebAPI.Data
{
	public class CatalogContext : ICatalogContext
	{
        public CatalogContext(IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DatabaseSettings__ConnectionString");
            var databaseName = Environment.GetEnvironmentVariable("DatabaseSettings__DatabaseName");
            var collectionName = Environment.GetEnvironmentVariable("DatabaseSettings__CollectionName");

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            Products = database.GetCollection<Product>(collectionName);
			CatalogContextSeed.SeedData(Products);
		}
        public IMongoCollection<Product> Products { get; }
	}
}
