using QuickOrderPagamento.Core.Application.Dtos;

namespace QuickOrderPagamento.Core.Application.UseCases.Pedido.Interfaces
{
    public interface IPagamentoObterUseCase : IBaseUseCase
    {
        Task<ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento>> ConsultarPagamento(int id);

    }
}
