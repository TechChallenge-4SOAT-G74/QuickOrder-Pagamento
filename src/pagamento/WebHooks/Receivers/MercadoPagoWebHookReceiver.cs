using Microsoft.AspNetCore.Mvc;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Adapters.Driving.Api.WebHooks.Receivers
{
    [ExcludeFromCodeCoverage]
    [Route("api/webhook/receiver/mercadopago")]
    [ApiController]
    public class MercadoPagoWebHookReceiver : ControllerBase
    {
        public readonly IPagamentoUseCase _pagamentoUseCase;

        public MercadoPagoWebHookReceiver(IPagamentoUseCase pagamentoUseCase)
        {
            _pagamentoUseCase = pagamentoUseCase;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Post(WebHookData data)
        {
            Console.Write(data);

            if (data.Type == "payment")

                await _pagamentoUseCase.VerificaPagamento(data);

            return Ok(true);
        }
    }
}
