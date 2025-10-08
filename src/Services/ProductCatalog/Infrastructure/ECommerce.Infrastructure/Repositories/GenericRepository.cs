using ECommerce.Application.Persistence.Contracts;
using ECommerce.Domain.Entity;
using ECommerce.Infrastructure.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IMongoCollection<T> collection;
        public GenericRepository(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            collection = database.GetCollection<T>(settings.CollectionName);
        }

        public async Task<T> Add(T entity)
        {
            if (entity is Product product)
            {
                product.Id = ObjectId.GenerateNewId().ToString();
            }
            await collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task Delete(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", ObjectId.Parse(id));
            var result = await collection.DeleteOneAsync(filter);
        }

        public async Task<T> Get(string id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll()
        {
            //return collection.Find(i => true).ToListAsync();
            return await collection.Find(Builders<T>.Filter.Empty).ToListAsync();
        }

        public async Task Update(string id, T entity)
        {
            //throw new NotImplementedException();

            var filter = Builders<T>.Filter.Eq("Id", ObjectId.Parse(id));
            var result = await collection.ReplaceOneAsync(filter, entity);
        }
    }
}
