using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;
using QuickOrderPagamento.Core.Application.Dtos;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces
{
    public interface IPagamentoUseCase : IBaseUseCase
    {
        Task VerificaPagamento(WebHookData whData);
        Task<bool> AtualizarStatusPagamento(string numeroPedido, int statusPagamento);
        Task<ServiceResult<PaymentQrCodeResponse>> GerarQrCodePagamento(int idPedido);
    }
}
