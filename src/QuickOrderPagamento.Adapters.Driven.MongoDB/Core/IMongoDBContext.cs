using MongoDB.Driver;

namespace QuickOrderPagamento.Adapters.Driven.MongoDB.Core
{
    public interface IMongoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
