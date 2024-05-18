namespace QuickOrderPagamento.Core.Domain.Entities
{
    public class Pagamento : EntityMongoBase
    {
        public int NumeroPedido { get; set; }
        public double Valor { get; set; }
        public DateTime Data { get; set; }
        public required string Status { get; set; }

    }
}
