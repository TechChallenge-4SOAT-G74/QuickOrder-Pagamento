namespace QuickOrderPagamento.Core.Application.Dtos
{
    public class PagamentoDto
    {
        public int NumeroPedido { get; set; }
        public DateTime DataHora { get; set; }
        public string? StatusPagamento { get; set; }
    }
}
