using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses
{
    [ExcludeFromCodeCoverage]
    public class PaymentQrCodeResponse
    {
        public string in_store_order_id { get; set; }
        public string qr_data { get; set; }
    }
}
