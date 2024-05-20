using System;
using System.Threading.Tasks;
using Moq;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Application.UseCases.Pedido.Interfaces;
using QuickOrderPagamento.Core.Domain.Adapters;
using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Core.Domain.Enums;
using QuickOrderPagamento.Infra.MQ;
using Xunit;

namespace QuickOrderPagamento.Core.Application.Tests.UseCases.Pagamento
{
    public class PagamentoUseCaseTests
    {
        private readonly PagamentoUseCase _pagamentoUseCase;
        private readonly Mock<IPagamentoObterUseCase> _pagamentoObterUseCaseMock;
        private readonly Mock<IMercadoPagoApi> _mercadoPagoApiMock;
        private readonly Mock<IPagamentoAtualizarUseCase> _pagamentoAtualizarUseCaseMock;
        private readonly Mock<IPagamentoCriarUseCase> _pagamentoCriarUseCaseMock;
        private readonly Mock<IRabbitMqPub<QuickOrderPagamento.Core.Domain.Entities.Pagamento>> _rabbitMqPubMock;

        public PagamentoUseCaseTests()
        {
            _pagamentoObterUseCaseMock = new Mock<IPagamentoObterUseCase>();
            _mercadoPagoApiMock = new Mock<IMercadoPagoApi>();
            _pagamentoAtualizarUseCaseMock = new Mock<IPagamentoAtualizarUseCase>();
            _pagamentoCriarUseCaseMock = new Mock<IPagamentoCriarUseCase>();
            _rabbitMqPubMock = new Mock<IRabbitMqPub<QuickOrderPagamento.Core.Domain.Entities.Pagamento>>();

            _pagamentoUseCase = new PagamentoUseCase(
                null,
                _pagamentoObterUseCaseMock.Object,
                _mercadoPagoApiMock.Object,
                _pagamentoCriarUseCaseMock.Object,
                _pagamentoAtualizarUseCaseMock.Object,
                _rabbitMqPubMock.Object
            );
        }

        [Fact]
        public async Task GerarQrCodePagamento_QuandoPagamentoNaoExistir_DeveCriarNovoPagamento()
        {
            // Arrange
            int idPedido = 1;
            double valorPedido = 100.0;
            var qrCodeResponse = new PaymentQrCodeResponse {};
            var serviceResult = new ServiceResult<PaymentQrCodeResponse> { Data = qrCodeResponse };
            _pagamentoObterUseCaseMock.Setup(uc => uc.ConsultarPagamento(idPedido)).ReturnsAsync(new ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento > { Data = null });
            _pagamentoCriarUseCaseMock.Setup(uc => uc.CriarNovoPagamento(idPedido, valorPedido)).ReturnsAsync(new 
                QuickOrderPagamento.Core.Domain.Entities.Pagamento(){ Status = "Novo" });
            _mercadoPagoApiMock.Setup(api => api.GeraQrCodePagamento(It.IsAny<PaymentQrCodeRequest>())).ReturnsAsync(qrCodeResponse);

            // Act
            var result = await _pagamentoUseCase.GerarQrCodePagamento(idPedido, valorPedido);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(qrCodeResponse, result.Data);
            _pagamentoObterUseCaseMock.Verify(uc => uc.ConsultarPagamento(idPedido), Times.Once);
            _pagamentoCriarUseCaseMock.Verify(uc => uc.CriarNovoPagamento(idPedido, valorPedido), Times.Once);
            _mercadoPagoApiMock.Verify(api => api.GeraQrCodePagamento(It.IsAny<PaymentQrCodeRequest>()), Times.Once);
        }

        [Fact]
        public async Task GerarQrCodePagamento_QuandoValorPedidoInvalido_DeveRetornarErro()
        {
            // Arrange
            int idPedido = 1;
            double valorPedido = 0.0;

            // Act
            var result = await _pagamentoUseCase.GerarQrCodePagamento(idPedido, valorPedido);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Valor do pedido inválido para pagamento.", result.Errors[0].Message);
        }
    }
}
