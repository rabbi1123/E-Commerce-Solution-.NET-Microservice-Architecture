using MongoDB.Driver;
using ProductCatalog.WebAPI.Entities;

namespace ProductCatalog.WebAPI.Data
{
	public interface ICatalogContext
	{
		IMongoCollection<Product> Products { get; }
	}
}
