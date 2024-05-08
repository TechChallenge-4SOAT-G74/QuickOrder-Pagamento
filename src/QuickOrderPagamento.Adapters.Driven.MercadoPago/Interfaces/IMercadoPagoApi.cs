using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;

namespace QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces
{
    public interface IMercadoPagoApi
    {
        Task<PaymentQrCodeResponse> GeraQrCodePagamento(PaymentQrCodeRequest request);
        Task<Payment> ObterPagamento(string id);
    }
}
