using QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Application.UseCases.Pedido.Interfaces;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Core.Domain.Enums;
using ItemRequest = QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests.Item;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento
{
    public class PagamentoUseCase : IPagamentoUseCase
    {
        
        private readonly IPagamentoStatusRepository _statusRepository;
        private readonly IPagamentoObterUseCase _pagamentoObterUseCase;
        private readonly IMercadoPagoApi _mercadoPagoApi;
        private readonly IPagamentoAtualizarUseCase _pagamentoAtualizarUseCase;
        private readonly IPagamentoCriarUseCase _pagamentoCriarUseCase;

        public PagamentoUseCase(IPagamentoStatusRepository statusRepository, 
                                IPagamentoObterUseCase pagamentoObterUseCase, 
                                IMercadoPagoApi mercadoPagoApi, 
                                IPagamentoCriarUseCase pagamentoCriarUseCase,
                                IPagamentoAtualizarUseCase pagamentoAtualizarUseCase)
        {
            _statusRepository = statusRepository;
            _pagamentoObterUseCase = pagamentoObterUseCase;
            _mercadoPagoApi = mercadoPagoApi;
            _pagamentoCriarUseCase = pagamentoCriarUseCase;
            _pagamentoAtualizarUseCase = pagamentoAtualizarUseCase;
        }

        public async Task VerificaPagamento(WebHookData whData)
        {
            
            var retorno = await _mercadoPagoApi.ObterPagamento(whData.Data.Id);

            var status = 0;
            switch (retorno.Status)
            {
                case "approved":
                    status = (int)EStatusPagamento.Aprovado;
                    break;
                /*case "pending":
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
                    break;*/
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

        // TODO: Salvar no banco de dados o status do pagamento e atualizar o status do pedido no topico de pedidos
        private async Task EnviarPedidoPagamento(SacolaDto sacolaDto, PaymentQrCodeResponse paymentQrCode)
        {
            try
            {
                var pagamentoStatus = new PagamentoStatus
                {
                    //ClienteId = Convert.ToInt32(sacolaDto.NumeroCliente),
                    NumeroPedido = Convert.ToInt32(sacolaDto.NumeroPedido),
                    DataAtualizacao = DateTime.Now,
                    Valor = sacolaDto.Valor,
                    StatusPagamento = EStatusPagamentoExtensions.ToDescriptionString(EStatusPagamento.Aguardando),
                    ProvedorPagamento = "Mercado Pago",
                    ChavePagamento = paymentQrCode.in_store_order_id,
                    QrCodePayment = paymentQrCode.qr_data
                };

                await _statusRepository.Create(pagamentoStatus);

            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                throw new ApplicationException("An error occurred while processing the payment.", ex);
            }
        }

        public async Task<ServiceResult<PaymentQrCodeResponse>> GerarQrCodePagamento(int idPedido, double valorPedido)
        {

            var result = new ServiceResult<PaymentQrCodeResponse>();

            try
            {
                var pagamento = await _pagamentoObterUseCase.ConsultarPagamento(idPedido);

                if (pagamento.Data == null)
                {
                    // Se o pagamento não existir, crie um novo pagamento aqui
                    pagamento = await _pagamentoCriarUseCase.CriarNovoPagamento(idPedido, valorPedido);

                    if (pagamento.Data == null)
                    {
                        // Se ainda assim o pagamento não puder ser criado, retorne um erro
                        result.AddError("Não foi possível criar um novo pagamento.");
                        return result;
                    }
                }

                if (valorPedido <= 0)
                {
                    result.AddError("Valor do pedido inválido para pagamento.");
                    return result;
                }

                var request = new PaymentQrCodeRequest
                {
                    description = $"Pedido {pagamento.Data.NumeroPedido}",
                    external_reference = pagamento.Data.NumeroPedido.ToString(),
                    items = new List<ItemRequest>()
                    {
                        new ItemRequest
                        {
                            title = $"Pedido {pagamento.Data.NumeroPedido}",
                            unit_price = Convert.ToInt32(valorPedido),
                            quantity = 1,
                            unit_measure = "unit",
                            total_amount = Convert.ToInt32(valorPedido),
                        },
                    },
                    title = "Product order",
                    total_amount = Convert.ToInt32(valorPedido),
                };

                var response = _mercadoPagoApi.GeraQrCodePagamento(request);

                var sacolaDto = new SacolaDto
                {
                    NumeroPedido = pagamento.Data.NumeroPedido.ToString(),
                    Valor = Convert.ToInt32(pagamento.Data.Valor)
                };

                pagamento.Data.Status = "Aguardando pagamento";
                await _pagamentoAtualizarUseCase.AtualizarPagamento(pagamento.Data.Id.ToString(), pagamento);

                await EnviarPedidoPagamento(sacolaDto, response.Result);

                result.Data = response.Result;

            }
            catch (Exception ex)
            {
                result.AddError(400, ex.Message);
            }

            return result;
        }
    }
}
