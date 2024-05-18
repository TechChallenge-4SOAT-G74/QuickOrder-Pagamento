using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuickOrderPagamento.Core.Domain.Entities;

namespace QuickOrderPagamento.Adapters.Driven.MongoDB.Core
{
    public class MongoDBContext : IMongoDBContext
    {
        private IMongoDatabase _db { get; set; }
        private MongoClient _client { get; set; }
        public IClientSessionHandle SessionHandle { get; set; }

        public MongoDBContext(IOptions<DatabaseMongoDBSettings> configuration)
        {
            _client = new MongoClient(configuration.Value.ConnectionString);
            _db = _client.GetDatabase(configuration.Value.DatabaseName);
        }


        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
