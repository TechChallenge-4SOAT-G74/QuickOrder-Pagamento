using QuickOrderPagamento.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickOrderPagamento.Core.Application.UseCases.Pagamento.Interfaces
{
    public interface IPagamentoCriarUseCase : IBaseUseCase
    {
        Task<QuickOrderPagamento.Core.Domain.Entities.Pagamento> CriarNovoPagamento(int idPedido, double valor);
    }
}
