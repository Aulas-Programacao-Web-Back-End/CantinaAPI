using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantinaAPI.DTOs
{
    public class PedidoCreateDTO
    {
        public int IdCliente { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
    }

    public class PedidoResponseDTO
    {
        public int IdPedido { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }
        public DateOnly DataPedido { get; set; }
    }
}