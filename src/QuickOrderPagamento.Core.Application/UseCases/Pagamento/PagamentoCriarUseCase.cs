using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Domain.Repositories;
using QuickOrderPagamento.Core.Application.Mappers;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento
{
    public class PagamentoCriarUseCase : IPagamentoCriarUseCase
    {
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoCriarUseCase(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<QuickOrderPagamento.Core.Domain.Entities.Pagamento> CriarNovoPagamento(int idPedido, double valor)
        {

            var novoPagamento = new PagamentoDto
            {
                NumeroPedido = idPedido,
                Valor = valor,
                StatusPagamento = "Novo",
                DataPagamento = new DateTime()
                };

            // Converta PagamentoDto para Pagamento
            var pagamentoEntity = novoPagamento.ToEntity();

            // Aqui você deve chamar o método para criar um novo pagamento no seu repositório
            await _pagamentoRepository.Create(pagamentoEntity);

            // Retorne o novo pagamento
            return pagamentoEntity;
        }
    }
}
