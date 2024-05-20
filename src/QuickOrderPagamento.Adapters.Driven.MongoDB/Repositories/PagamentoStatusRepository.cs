using QuickOrderPagamento.Adapters.Driven.MongoDB.Core;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Adapters.Driven.MongoDB.Repositories
{
    [ExcludeFromCodeCoverage]
    public class PagamentoStatusRepository : BaseMongoDBRepository<PagamentoStatus>, IPagamentoStatusRepository
    {
        public PagamentoStatusRepository(IMongoDBContext mondoDBContext) : base(mondoDBContext)
        {
        }
    }
}
