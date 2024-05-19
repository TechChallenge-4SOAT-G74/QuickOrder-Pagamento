using Microsoft.AspNetCore.Http;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pedido.Interfaces;
using QuickOrderPagamento.Core.Domain.Repositories;

namespace QuickOrderPagamento.Core.Application.UseCases.Pedido
{
    public class PagamentoObterUseCase : IPagamentoObterUseCase
    {
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoObterUseCase(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento>> ConsultarPagamento(int numeroPedido)
        {
            var result = new ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento>();

            var pagamento = await _pagamentoRepository.GetPagamentoByNumeroPedidoAsync(numeroPedido);
            if (pagamento == null)
            {
                result.AddError("Pagamento não encontrado.");
                result.CodeId = StatusCodes.Status404NotFound;
                return result;
            }

            result.Data = pagamento;
            return result;
        }
    }
}
