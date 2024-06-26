﻿using System.Diagnostics.CodeAnalysis;

namespace QuickOrderPagamento.Core.Application.Dtos
{
    [ExcludeFromCodeCoverage]
    public class PagamentoDto
    {
        public string Id { get; set; }
        public int NumeroPedido { get; set; }
        public DateTime DataPagamento { get; set; }
        public required string StatusPagamento { get; set; }
        public double Valor { get; set; }
    }
}
