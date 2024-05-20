using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento
{
    [ExcludeFromCodeCoverage]
    internal class SacolaDto
    {
        public object NumeroCliente { get; set; }
        public object NumeroPedido { get; set; }
        public int Valor { get; set; }
    }
}