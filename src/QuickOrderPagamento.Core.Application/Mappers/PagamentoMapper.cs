using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Core.Application.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class PagamentoMapper
    {
        public static Pagamento ToEntity(this PagamentoDto dto)
        {
            return new Pagamento
            {
                NumeroPedido = dto.NumeroPedido,
                Valor = dto.Valor,
                Status = dto.StatusPagamento,
                // Adicione outros campos conforme necessário
            };
        }
    }
}
