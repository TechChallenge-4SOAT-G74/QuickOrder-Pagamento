using Xunit;
using QuickOrderPagamento.Core.Domain.Enums;
using System;
using System.ComponentModel;

namespace QuickOrderPagamento.Core.Domain.Tests.Enums
{
    public class EStatusPagamentoTests
    {
        [Fact]
        public void EStatusPagamento_Enum_DeveRetornarDescricaoCorreta()
        {
            Assert.Equal("Aguardando Pagamento", EStatusPagamento.Aguardando.ToDescriptionString());
            Assert.Equal("processando pagamento", EStatusPagamento.Processando.ToDescriptionString());
            Assert.Equal("Pagamento Aprovado", EStatusPagamento.Aprovado.ToDescriptionString());
            Assert.Equal("Pagamento Negado", EStatusPagamento.Negado.ToDescriptionString());
        }
    }

    public class EStatusPagamentoExtensionsTests
    {
        [Fact]
        public void ToDescriptionString_DeveRetornarDescricaoCorreta()
        {
            Assert.Equal("Aguardando Pagamento", EStatusPagamentoExtensions.ToDescriptionString(EStatusPagamento.Aguardando));
            Assert.Equal("processando pagamento", EStatusPagamentoExtensions.ToDescriptionString(EStatusPagamento.Processando));
            Assert.Equal("Pagamento Aprovado", EStatusPagamentoExtensions.ToDescriptionString(EStatusPagamento.Aprovado));
            Assert.Equal("Pagamento Negado", EStatusPagamentoExtensions.ToDescriptionString(EStatusPagamento.Negado));
        }
    }
}
