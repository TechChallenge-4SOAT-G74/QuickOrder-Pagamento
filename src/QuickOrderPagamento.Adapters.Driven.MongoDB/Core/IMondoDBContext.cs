using MongoDB.Driver;

namespace QuickOrderPagamento.Adapters.Driven.MongoDB.Core
{
    public interface IMondoDBContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
