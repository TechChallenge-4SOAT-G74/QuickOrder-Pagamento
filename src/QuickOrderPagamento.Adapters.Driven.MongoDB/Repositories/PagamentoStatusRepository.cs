using QuickOrderPagamento.Adapters.Driven.MongoDB.Core;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Entities;

namespace QuickOrderPagamento.Adapters.Driven.MongoDB.Repositories
{
    public class PagamentoStatusRepository : BaseMongoDBRepository<PagamentoStatus>, IPagamentoStatusRepository
    {
        public PagamentoStatusRepository(IMondoDBContext mondoDBContext) : base(mondoDBContext)
        {
        }
    }
}
