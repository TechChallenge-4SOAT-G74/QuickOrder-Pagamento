using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Core.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class MercadoPagoSettings
    {
        public string AccessToken { get; set; }
        public int User_id { get; set; }
        public string External_pos_id { get; set; }
        public string NotificationUrl { get; set; }
    }
}
