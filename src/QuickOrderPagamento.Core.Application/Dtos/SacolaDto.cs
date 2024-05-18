namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento
{
    internal class SacolaDto
    {
        public object NumeroCliente { get; set; }
        public object NumeroPedido { get; set; }
        public int Valor { get; set; }
    }
}