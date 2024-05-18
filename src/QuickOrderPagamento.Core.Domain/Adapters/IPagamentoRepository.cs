using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Entities;

namespace QuickOrderPagamento.Core.Domain.Repositories
{
    public interface IPagamentoRepository : IBaseRepository, IBaseMongoDBRepository<Pagamento>
    {
        Task<Pagamento> GetPagamentoByNumeroPedidoAsync(int numeroPedido);
        Task<Pagamento> GetPagamentoByIdAsync(int id);

        Task Update(Pagamento pagamento);
    }
}