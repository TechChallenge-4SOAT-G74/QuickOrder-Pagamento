using Flurl.Http;
using Microsoft.Extensions.Options;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;
using QuickOrderPagamento.Core.Domain.Entities;

namespace QuickOrderPagamento.Adapters.Driven.MercadoPago
{
    public class MercadoPagoApi : IMercadoPagoApi
    {
        private readonly string _accessToken;
        private readonly int _user_id;
        private readonly string _external_pos_id;
        private readonly string _notification_url;

        public MercadoPagoApi(IOptions<MercadoPagoSettings> configuration)
        {
            _accessToken = configuration.Value.AccessToken;
            _user_id = configuration.Value.User_id;
            _external_pos_id = configuration.Value.External_pos_id;
            _notification_url = configuration.Value.NotificationUrl;
        }

        public async Task<PaymentQrCodeResponse> GeraQrCodePagamento(PaymentQrCodeRequest request)
        {
            try
            {
                string url = $"https://api.mercadopago.com/instore/orders/qr/seller/collectors/{_user_id}/pos/{_external_pos_id}/qrs";

                request.notification_url = _notification_url;

                var result = await url
                       .WithHeader("Content-Type", "application/json")
                       .WithOAuthBearerToken(_accessToken)
                       .PostJsonAsync(request)
                       .ReceiveJson<PaymentQrCodeResponse>();

                return result;
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Console.Write($"Error returned from {ex.Call.Request.Url}: {error}");

                throw new Exception($"Error returned from {ex.Call.Request.Url}: {error}");
            }
        }

        public async Task<Payment> ObterPagamento(string id)
        {
            try
            {
                string url = $"https://api.mercadopago.com/v1/payments/{id}";

                var result = await url
                       .WithHeader("Content-Type", "application/json")
                       .WithOAuthBearerToken(_accessToken)
                       .GetAsync()
                       .ReceiveJson<Payment>();

                return result;
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync<object>();
                Console.Write($"Error returned from {ex.Call.Request.Url}: {error}");

                throw new Exception($"Error returned from {ex.Call.Request.Url}: {error}");
            }
        }
    }
}
