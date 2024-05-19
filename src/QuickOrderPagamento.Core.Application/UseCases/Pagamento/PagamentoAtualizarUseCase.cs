using MongoDB.Bson;
using QuickOrderPagamento.Core.Application.Dtos;
using QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces;
using QuickOrderPagamento.Core.Domain.Repositories;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento
{
    public class PagamentoAtualizarUseCase : IPagamentoAtualizarUseCase
    {
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoAtualizarUseCase(IPagamentoRepository pagamentoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task AtualizarPagamento(string id, ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento> pagamento)
        {
            if (pagamento == null)
            {
                throw new ArgumentNullException(nameof(pagamento), "Pagamento não pode ser nulo.");
            }

            await _pagamentoRepository.Update(pagamento.Data);
        }
    }
}


