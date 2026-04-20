using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantinaAPI.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int IdCliente { get; set; }
        public int IdProduto { get; set; }
        public DateOnly DataPedido { get; set; }
        public int Quantidade { get; set; }
        public double ValorTotal { get; set; }

        // Propriedades de navegação
        public Cliente Cliente { get; set; } = null!;
        public Produto Produto { get; set; } = null!;
    }
}