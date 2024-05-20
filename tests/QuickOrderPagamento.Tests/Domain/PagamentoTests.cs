using QuickOrderPagamento.Core.Domain.Entities;
using Xunit;

namespace QuickOrderPagamento.Tests.Domain.Tests
{
    public class PagamentoTests
    {
        [Fact]
        public void Pagamento_DeveInicializarCorretamente()
        {
            // Arrange
            var numeroPedido = 1;
            var valor = 150.00;
            var data = DateTime.Now;
            var status = "Novo";

            // Act
            var pagamento = new Pagamento
            {
                NumeroPedido = numeroPedido,
                Valor = valor,
                Data = data,
                Status = status
            };

            // Assert
            Assert.Equal(numeroPedido, pagamento.NumeroPedido);
            Assert.Equal(valor, pagamento.Valor);
            Assert.Equal(data, pagamento.Data);
            Assert.Equal(status, pagamento.Status);
        }
    }
}
