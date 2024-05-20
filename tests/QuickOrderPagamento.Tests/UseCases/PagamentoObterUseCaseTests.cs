using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pedido;
using QuickOrderPagamento.Core.Application.UseCases.Pedido.Interfaces;
using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Core.Domain.Repositories;
using Xunit;

namespace QuickOrderPagamento.Core.Application.Tests.UseCases.Pedido
{
    public class PagamentoObterUseCaseTests
    {
        private IPagamentoObterUseCase _pagamentoObterUseCase;
        private Mock<IPagamentoRepository> _pagamentoRepositoryMock;

        public PagamentoObterUseCaseTests()
        {
            _pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            _pagamentoObterUseCase = new PagamentoObterUseCase(_pagamentoRepositoryMock.Object);
        }

        [Fact]
        public async Task ConsultarPagamento_QuandoPagamentoExistir_DeveRetornarPagamento()
        {
            // Arrange
            int numeroPedido = 123;
            var pagamento = new QuickOrderPagamento.Core.Domain.Entities.Pagamento { NumeroPedido = numeroPedido, Status = "Novo" };
            _pagamentoRepositoryMock.Setup(repo => repo.GetPagamentoByNumeroPedidoAsync(numeroPedido)).ReturnsAsync(pagamento);

            // Act
            var result = await _pagamentoObterUseCase.ConsultarPagamento(numeroPedido);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(pagamento, result.Data);
        }

        [Fact]
        public async Task ConsultarPagamento_QuandoPagamentoNaoExistir_DeveRetornarErro()
        {
            // Arrange
            int numeroPedido = 123;
            _pagamentoRepositoryMock.Setup(repo => repo.GetPagamentoByNumeroPedidoAsync(numeroPedido)).ReturnsAsync((QuickOrderPagamento.Core.Domain.Entities.Pagamento)null);

            // Act
            var result = await _pagamentoObterUseCase.ConsultarPagamento(numeroPedido);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Pagamento não encontrado.", result.Errors[0].Message);
            Assert.Equal(StatusCodes.Status404NotFound, result.CodeId);
        }
    }
}
