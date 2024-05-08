using QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Core.Domain.Enums;
using ItemRequest = QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests.Item;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento
{
    public class PagamentoUseCase : IPagamentoUseCase
    {
        
        private readonly IPagamentoStatusRepository _statusRepository;
        //private readonly IPedidoObterUseCase _pedidoObterUseCase;
        private readonly IMercadoPagoApi _mercadoPagoApi;
        //private readonly IPedidoStatusRepository _pedidoStatusRepository;

        
        public PagamentoUseCase(IPagamentoStatusRepository statusRepository, IMercadoPagoApi mercadoPagoApi)
        {
            _statusRepository = statusRepository;
            //_pedidoObterUseCase = pedidoObterUseCase;
            _mercadoPagoApi = mercadoPagoApi;
            //_pedidoStatusRepository = pedidoStatusRepository;
        }
        

        public async Task VerificaPagamento(WebHookData whData)
        {
            /*
            var retorno = await _mercadoPagoApi.ObterPagamento(whData.Data.Id);

            var status = 0;
            switch (retorno.Status)
            {
                case "approved":
                    status = (int)EStatusPagamento.Aprovado;
                    break;
                case "pending":
                    status = (int)EStatusPagamento.aguardando;
                    break;
                case "authorized":
                    status = (int)EStatusPagamento.processando;
                    break;
                case "in_process":
                    status = (int)EStatusPagamento.processando;
                    break;
                case "in_mediation":
                    status = (int)EStatusPagamento.processando;
                    break;
                case "rejected":
                    status = (int)EStatusPagamento.Negado;
                    break;
                case "cancelled":
                    status = (int)EStatusPagamento.Negado;
                    break;
                case "refunded":
                    status = (int)EStatusPagamento.Negado;
                    break;
                case "charged_back":
                    status = (int)EStatusPagamento.Negado;
                    break;
            }

            await AtualizarStatusPagamento(whData.Data.Id, status);
            */
        }


        public async Task<bool> AtualizarStatusPagamento(string numeroPedido, int statusPagamento)
        {
            /*
            var pedido = await _statusRepository.GetValue("NumeroPedido", numeroPedido);

            if (pedido != null)
            {
                var pagamentoStatus = new PagamentoStatus
                {
                    Id = pedido.Id,
                    NumeroPedido = pedido.NumeroPedido,
                    clienteId = pedido.clienteId,
                    StatusPagamento = EStatusPagamentoExtensions.ToDescriptionString((EStatusPagamento)statusPagamento),
                    DataAtualizacao = DateTime.Now
                };
                _statusRepository.Update(pagamentoStatus);

                if ((EStatusPagamento)statusPagamento == EStatusPagamento.Aprovado)
                {
                    var pedidoStatus = new PedidoStatus((int)Convert.ToUInt32(pedido.NumeroPedido), EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.Recebido), DateTime.Now);
                    _pedidoStatusRepository.Update(pedidoStatus);
                }

            }
            */
            return true;
           
        }

        /*
        private async Task EnviarPedidoPagamento(SacolaDto sacolaDto, PaymentQrCodeResponse paymentQrCode)
        {
            /*
            var pagamentoStatus = new PagamentoStatus
            {
                clienteId = (int?)Convert.ToUInt32(sacolaDto.NumeroCliente),
                NumeroPedido = (int)Convert.ToUInt32(sacolaDto.NumeroPedido),
                DataAtualizacao = DateTime.Now,
                Valor = sacolaDto.Valor,
                StatusPagamento = EStatusPagamentoExtensions.ToDescriptionString(EStatusPagamento.aguardando),
                ProvedorPagamento = "Mercado Pago",
                ChavePagamento = paymentQrCode.in_store_order_id,
                QrCodePayment = paymentQrCode.qr_data
            };
            await _statusRepository.Create(pagamentoStatus);

            var pedidoStatus = new PedidoStatus((int)Convert.ToUInt32(sacolaDto.NumeroPedido), EStatusPedidoExtensions.ToDescriptionString(EStatusPedido.PendentePagamento), DateTime.Now);
            _pedidoStatusRepository.Update(pedidoStatus);
            
        }
        */

        
        public async Task<ServiceResult<PaymentQrCodeResponse>> GerarQrCodePagamento(int idPedido)
        {
            
            var result = new ServiceResult<PaymentQrCodeResponse>();

            /*
            try
            {
                var pedido = await _pedidoObterUseCase.ConsultarPedido(idPedido);

                if (pedido == null)
                {
                    result.AddError("Pedido não localizado.");
                    return result;
                }

                var request = new PaymentQrCodeRequest
                {
                    description = $"Pedido {pedido.Data.NumeroCliente}",
                    external_reference = pedido.Data.NumeroPedido.ToString(),
                    items = new List<ItemRequest>()
                    {
                        new ItemRequest
                        {
                            title = $"Pedido {pedido.Data.NumeroCliente}",
                            unit_price = Convert.ToInt32(pedido.Data.ValorPedido),
                            quantity = 1,
                            unit_measure = "unit",
                            total_amount = Convert.ToInt32(pedido.Data.ValorPedido),
                        },
                    },
                    title = "Product order",
                    total_amount = Convert.ToInt32(pedido.Data.ValorPedido),
                };

                if (Convert.ToInt32(pedido.Data.ValorPedido) == 0)
                {
                    result.AddError("Pedido não possui valor para pagamento.");
                    return result;
                }

                var response = _mercadoPagoApi.GeraQrCodePagamento(request);

                var sacolaDto = new SacolaDto
                {
                    NumeroCliente = pedido.Data.NumeroCliente.ToString(),
                    NumeroPedido = pedido.Data.NumeroPedido.ToString(),
                    Valor = Convert.ToInt32(pedido.Data.ValorPedido)
                };

                await EnviarPedidoPagamento(sacolaDto, response.Result);

                result.Data = response.Result;
            }
            catch (Exception ex)
            {
                result.AddError(400, ex.Message);
            }
            */

            return result;
        }
        
    }
}
