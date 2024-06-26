﻿using MongoDB.Bson;
using MongoDB.Driver;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Entities;
using ServiceStack;
using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Adapters.Driven.MongoDB.Core
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseMongoDBRepository<TEntity> : IBaseMongoDBRepository<TEntity> where TEntity : EntityMongoBase
    {
        protected readonly IMongoDBContext _mondoDBContext;
        protected IMongoCollection<TEntity> _dbCollection;

        protected BaseMongoDBRepository(IMongoDBContext mondoDBContext)
        {
            _mondoDBContext = mondoDBContext;
            _dbCollection = _mondoDBContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public async Task Create(TEntity obj)
        {
            if (obj == null) throw new ArgumentNullException(typeof(TEntity).Name + " object is null");
            await _dbCollection.InsertOneAsync(obj);
        }

        public void Delete(string id)
        {
            var objectId = new ObjectId(id);
            _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
        }

        public async Task<TEntity> Get(string id)
        {
            var objectId = new ObjectId(id);
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", objectId);
            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetValue(string column, string value)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(column, value);
            return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty);
            return await all.ToListAsync();
        }

        public void Update(TEntity obj)
        {
            _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj);
        }
    }
}
