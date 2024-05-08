namespace QuickOrderPagamento.Core.Domain.Entities
{
    public class PagamentoStatus : EntityMongoBase
    {
        public int NumeroPedido { get; set; }
        public int? clienteId { get; set; }
        public double Valor { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string? StatusPagamento { get; set; }
        public string? ProvedorPagamento { get; set; }
        public string? ChavePagamento { get; set; }
        public string? QrCodePayment { get; set; }
    }
}
