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
using QuickOrderPagamento.Infra.MQ;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento
{
    public class PagamentoUseCase : IPagamentoUseCase
    {
        
        private readonly IPagamentoStatusRepository _statusRepository;
        private readonly IPagamentoObterUseCase _pagamentoObterUseCase;
        private readonly IMercadoPagoApi _mercadoPagoApi;
        private readonly IPagamentoAtualizarUseCase _pagamentoAtualizarUseCase;
        private readonly IPagamentoCriarUseCase _pagamentoCriarUseCase;
        private readonly IRabbitMqPub<Domain.Entities.Pagamento> _rabbitMqPub;

        public PagamentoUseCase(IPagamentoStatusRepository statusRepository, 
                                IPagamentoObterUseCase pagamentoObterUseCase,
                                IMercadoPagoApi mercadoPagoApi,
                                IPagamentoCriarUseCase pagamentoCriarUseCase,
                                IPagamentoAtualizarUseCase pagamentoAtualizarUseCase,
                                IRabbitMqPub<Domain.Entities.Pagamento> rabbitMqPub)
        {
            _statusRepository = statusRepository;
            _pagamentoObterUseCase = pagamentoObterUseCase;
            _mercadoPagoApi = mercadoPagoApi;
            _pagamentoCriarUseCase = pagamentoCriarUseCase;
            _pagamentoAtualizarUseCase = pagamentoAtualizarUseCase;
            _rabbitMqPub = rabbitMqPub;
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

            /* TODO: APAGAR DEPOIS - só para testes -------------------------------- */
            //var webHookData = new WebHookData
            //{
            //    Data = new Data
            //    {
            //        Id = idPedido.ToString()
            //    }
            //};
            //await VerificaPagamento(webHookData);
            /* só para testes ------------------------------------------------------ */

            return result;
        }

        public async Task VerificaPagamento(WebHookData whData)
        {

            //var retorno = await _mercadoPagoApi.ObterPagamento(whData.Data.Id);

            /* TODO: APAGAR DEPOIS - só para testes -------------------------------- */
            var retorno = new Payment
            {
                Status = "approved"
            };
            /* só para testes ------------------------------------------------------ */

            var status = 0;
            switch (retorno.Status)
            {
                case "approved":
                    status = (int)EStatusPagamento.Aprovado;
                    break;
                case "pending":
                    status = (int)EStatusPagamento.Aguardando;
                    break;
                case "authorized":
                    status = (int)EStatusPagamento.Processando;
                    break;
                case "in_process":
                    status = (int)EStatusPagamento.Processando;
                    break;
                case "in_mediation":
                    status = (int)EStatusPagamento.Processando;
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
        }

        public async Task<bool> AtualizarStatusPagamento(string numeroPedido, int statusPagamento)
        {
            var pagamento = await _pagamentoObterUseCase.ConsultarPagamento(Int32.Parse(numeroPedido));
                
            if (pagamento.Data != null)
            {
                EStatusPagamento statusEnum = (EStatusPagamento)statusPagamento;
                pagamento.Data.Status = statusEnum.ToDescriptionString();
                await _pagamentoAtualizarUseCase.AtualizarPagamento(pagamento.Data.Id.ToString(), pagamento);

                if ((EStatusPagamento)statusPagamento == EStatusPagamento.Aprovado)
                {
                    Console.WriteLine("Pagamento aprovado");
                    _rabbitMqPub.Publicar(pagamento.Data, "Pagamento", "Pagamento_Confirmado");
                }
            }

            return true;
        }

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
    }
}
