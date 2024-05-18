using QuickOrderPagamento.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces
{
    public interface IPagamentoAtualizarUseCase : IBaseUseCase
    {
        //Task AtualizarPagamento(ServiceResult<PagamentoDto> pagamento);
        Task AtualizarPagamento(string id, ServiceResult<QuickOrderPagamento.Core.Domain.Entities.Pagamento> pagamento);

    }
}
