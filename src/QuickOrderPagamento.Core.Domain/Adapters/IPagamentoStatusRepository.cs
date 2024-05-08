using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Core.Domain.Repositories;

namespace QuickOrderPagamento.Core.Domain.Adapters
{
    public interface IPagamentoStatusRepository : IBaseRepository, IBaseMongoDBRepository<PagamentoStatus>
    {
    }
}
