using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses
{
    [ExcludeFromCodeCoverage]
    public class Payment
    {
        public int Id { get; set; }
        public DateTime Date_created { get; set; }
        public DateTime Date_approved { get; set; }
        public string? Payment_type_id { get; set; }
        public string? Status { get; set; }
        public int Transaction_amount { get; set; }
        public int Transaction_amount_refunded { get; set; }
    }
}