namespace QuickOrderPagamento.Core.Domain.Entities
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
    }

    public class DatabaseMongoDBSettings : DatabaseSettings
    {
        public string DatabaseName { get; set; } = null!;

    }
}
