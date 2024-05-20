using System;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Interfaces;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Responses;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento;
using QuickOrderPagamento.Core.Application.UseCases.Pedido.Interfaces;
using QuickOrderPagamento.Core.Domain.Entities;
using Xunit;
using QuickOrderPagamento.Adapters.Driven.MercadoPago.Requests;

namespace QuickOrderPagamento.Tests.UseCases
{
    public class PagamentoUseCaseTests
    {
        private readonly AutoMocker _mocker;
        private readonly PagamentoUseCase _pagamentoUseCase;

        public PagamentoUseCaseTests()
        {
            _mocker = new AutoMocker();
            _pagamentoUseCase = _mocker.CreateInstance<PagamentoUseCase>();
        }

        [Fact]
        public async Task GerarQrCodePagamento_DeveRetornarErroSeValorPedidoInvalido()
        {
            // Arrange
            var idPedido = 1;
            var valorPedido = 0.0; // Valor inválido

            // Act
            var result = await _pagamentoUseCase.GerarQrCodePagamento(idPedido, valorPedido);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.NotNull(result.Errors);
            Assert.NotEmpty(result.Errors);
        }
    }
}
