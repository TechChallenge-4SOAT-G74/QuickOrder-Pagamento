using QuickOrderPagamento.Core.Domain.Entities;
using Xunit;

namespace QuickOrderPagamento.Tests.Domain.Tests
{
    public class PagamentoStatusTests
    {
        [Fact]
        public void PagamentoStatus_DeveInicializarCorretamente()
        {
            // Arrange
            var numeroPedido = 1;
            var clienteId = 2;
            var valor = 150.75;
            var dataAtualizacao = DateTime.Now;
            var statusPagamento = "Novo";
            var provedorPagamento = "ProvedorTeste";
            var chavePagamento = "ChaveTeste";
            var qrCodePayment = "QRCodeTeste";

            // Act
            var pagamentoStatus = new PagamentoStatus
            {
                NumeroPedido = numeroPedido,
                clienteId = clienteId,
                Valor = valor,
                DataAtualizacao = dataAtualizacao,
                StatusPagamento = statusPagamento,
                ProvedorPagamento = provedorPagamento,
                ChavePagamento = chavePagamento,
                QrCodePayment = qrCodePayment
            };

            // Assert
            Assert.Equal(numeroPedido, pagamentoStatus.NumeroPedido);
            Assert.Equal(clienteId, pagamentoStatus.clienteId);
            Assert.Equal(valor, pagamentoStatus.Valor);
            Assert.Equal(dataAtualizacao, pagamentoStatus.DataAtualizacao);
            Assert.Equal(statusPagamento, pagamentoStatus.StatusPagamento);
            Assert.Equal(provedorPagamento, pagamentoStatus.ProvedorPagamento);
            Assert.Equal(chavePagamento, pagamentoStatus.ChavePagamento);
            Assert.Equal(qrCodePayment, pagamentoStatus.QrCodePayment);
        }
    }
}
