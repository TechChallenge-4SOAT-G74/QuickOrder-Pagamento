using MongoDB.Driver;
using QuickOrderPagamento.Adapters.Driven.MongoDB.Core;
using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Core.Domain.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Infrastructure.Data
{
    [ExcludeFromCodeCoverage]
    public class PagamentoRepository : BaseMongoDBRepository<Pagamento>, IPagamentoRepository
    {
        private readonly IMongoCollection<Pagamento> _pagamentos;

        public PagamentoRepository(IMongoDBContext mongoDBContext) : base(mongoDBContext)
        {
            _pagamentos = mongoDBContext.GetCollection<Pagamento>("Pagamento");
        }

        public async Task<Pagamento> GetPagamentoByIdAsync(int id)
        {
            var filter = Builders<Pagamento>.Filter.Eq("_id", id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task Update(Pagamento pagamento)
        {
            var filter = Builders<Pagamento>.Filter.Eq(p => p.Id, pagamento.Id);
            await _pagamentos.ReplaceOneAsync(filter, pagamento);
        }

        public async Task<Pagamento> GetPagamentoByNumeroPedidoAsync(int numeroPedido)
        {
            var filter = Builders<Pagamento>.Filter.Eq(p => p.NumeroPedido, numeroPedido);
            var pagamento = await _pagamentos.Find(filter).FirstOrDefaultAsync();

            return pagamento;
        }

    }
}