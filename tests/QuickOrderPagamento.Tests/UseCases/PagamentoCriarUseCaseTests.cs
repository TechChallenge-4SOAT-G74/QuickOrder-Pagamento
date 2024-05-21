using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento;
using QuickOrderPagamento.Core.Domain.Entities;
using QuickOrderPagamento.Core.Domain.Repositories;
using Xunit;

namespace QuickOrderPagamento.Tests.UseCases
{
    public class PagamentoCriarUseCaseTests
    {
        [Fact]
        public async Task CriarNovoPagamento_DeveCriarNovoPagamentoCorretamente()
        {
            // Arrange
            var idPedido = 1;
            var valor = 150.0;
            var pagamentoDto = new PagamentoDto
            {
                NumeroPedido = idPedido,
                Valor = valor,
                StatusPagamento = "Novo",
                DataPagamento = DateTime.Now
            };
            var pagamentoEntity = new Pagamento
            {
                NumeroPedido = idPedido,
                Valor = valor,
                Status = "Novo",
                Data = DateTime.Now
            };

            var pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            pagamentoRepositoryMock.Setup(repo => repo.Create(It.IsAny<Pagamento>())).Returns(Task.CompletedTask);

            var useCase = new PagamentoCriarUseCase(pagamentoRepositoryMock.Object);

            // Act
            var result = await useCase.CriarNovoPagamento(idPedido, valor);
            TimeSpan timeSpan = DateTime.Now - result.Data;

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<QuickOrderPagamento.Core.Domain.Entities.Pagamento>();
            result.NumeroPedido.Should().Be(idPedido);
            result.Valor.Should().Be(valor);
            result.Status.Should().Be("Novo");

            pagamentoRepositoryMock.Verify(repo => repo.Create(It.IsAny<Pagamento>()), Times.Once);
        }
    }
}
