using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests
{
    [ExcludeFromCodeCoverage]
    public class PaymentQrCodeRequest
    {
        public PaymentQrCodeRequest()
        {
            items = new List<Item>();
        }
        public CashOut cash_out { get; set; }
        public string description { get; set; }
        public string external_reference { get; set; }
        public List<Item> items { get; set; }
        public string notification_url { get; set; }
        public Sponsor sponsor { get; set; }
        public string title { get; set; }
        public int total_amount { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CashOut
    {
        public int amount { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Item
    {
        public string sku_number { get; set; }
        public string category { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int unit_price { get; set; }
        public int quantity { get; set; }
        public string unit_measure { get; set; }
        public int total_amount { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Sponsor
    {
        public int id { get; set; }
    }
}
