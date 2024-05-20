using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests
{
    [ExcludeFromCodeCoverage]
    public class WebHookData
    {
        public string Action { get; set; }
        public string Api_version { get; set; }
        public Data Data { get; set; }
        public DateTime Date_created { get; set; }
        public string Id { get; set; }
        public bool Live_mode { get; set; }
        public string Type { get; set; }
        public int User_id { get; set; }


    }

    [ExcludeFromCodeCoverage]
    public class Data
    {
        public string Id { get; set; }
    }
}
