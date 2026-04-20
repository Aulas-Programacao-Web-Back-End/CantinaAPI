using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantinaAPI.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public required string Descricao { get; set; }
        public required double Preco { get; set; }
        public required string Categoria { get; set; }

        public List<Pedido> Pedidos { get; set; } = [];
    }
}