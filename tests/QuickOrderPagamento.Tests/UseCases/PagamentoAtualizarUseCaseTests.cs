using Moq;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Domain.Repositories;
using QuickOrderPagamento.Core.Domain.Entities;
using Xunit;
using MongoDB.Bson;
using FluentAssertions;

namespace QuickOrderPagamento.Tests.UseCases
{
    public class PagamentoAtualizarUseCaseTests
    {
        [Fact]
        public async Task AtualizarPagamento_DeveAtualizarPagamentoCorretamente()
        {
            // Arrange
            var pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            var useCase = new PagamentoAtualizarUseCase(pagamentoRepositoryMock.Object);

            var id = "123";
            var pagamento = new ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento>
            {
                Data = new Pagamento
                {
                    Id = ObjectId.Parse("61533f3a43292401985c40ba"),
                    Status = "Aprovado"
                }
            };

            // Act
            await useCase.AtualizarPagamento(id, pagamento);

            // Assert
            pagamentoRepositoryMock.Verify(repo => repo.Update(pagamento.Data), Times.Once);
        }

        [Fact]
        public async Task AtualizarPagamento_DeveLancarExcecaoSePagamentoForNulo()
        {
            // Arrange
            var pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
            var useCase = new PagamentoAtualizarUseCase(pagamentoRepositoryMock.Object);

            string id = "123";
            ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento> pagamento = null;

            // Act
            Func<Task> action = async () => await useCase.AtualizarPagamento(id, pagamento);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Pagamento não pode ser nulo.");
        }
    }
}
