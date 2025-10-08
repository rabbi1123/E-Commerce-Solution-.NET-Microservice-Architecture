using ECommerce.Application.Persistence.Contracts;
using ECommerce.Domain.Entity;
using ECommerce.Infrastructure.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly IMongoCollection<Product> collection;
        public ProductRepository(IDatabaseSettings settings, IMongoClient mongoClient) : base(settings, mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            collection = database.GetCollection<Product>(settings.CollectionName);
        }
    }
}
