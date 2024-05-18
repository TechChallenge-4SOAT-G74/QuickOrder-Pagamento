using Microsoft.AspNetCore.Mvc;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;

namespace QuickOrderPagamento.Adapters.Driving.Api.Controllers.Pagamento
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : CustomController<PagamentoController>
    {
        private readonly IPagamentoUseCase _pagamentoUseCase;
        public PagamentoController(ILogger<PagamentoController> logger, IPagamentoUseCase pagamentoUseCase) : base(logger)
        {
            _pagamentoUseCase = pagamentoUseCase;
        }

        /// <summary>
        /// Gerar QrCode de pagamento para o Pedido do Cliente
        /// </summary>
        /// <param name="idPedido"></param>
        /// <param name="valorPedido"></param>
        /// <returns></returns>
        [HttpGet("gerarqrcodepagamento/{idPedido}")]
        public async Task<IActionResult> GerarQrCodePagamento(int idPedido, [FromQuery] double valorPedido)
        {
            return Result(await _pagamentoUseCase.GerarQrCodePagamento(idPedido, valorPedido));
        }

    }
}
