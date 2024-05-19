namespace QuickOrderPagamento.Infra.MQ
{
    public interface IProcessaEvento
    {
        void Processa(string mensagem);
    }
}
