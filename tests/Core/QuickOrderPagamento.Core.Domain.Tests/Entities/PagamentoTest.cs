using QuickOrderPagamento.Core.Domain.Entities;

namespace QuickOrderPagamento.Core.Domain.Tests.Entities
{
    public class PagamentoTest
    {
        [Fact]
        public void CriarPagamento_ComValoresValidos_DeveCriarObjetoCorretamente()
        {
            // Arrange
            int numeroPedido = 12345;
            double valor = 100.50;
            DateTime data = DateTime.Now;
            string status = "Novo";

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
