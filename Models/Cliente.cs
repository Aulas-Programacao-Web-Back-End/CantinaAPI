using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantinaAPI.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public required string Nome { get; set; }
        public string? Turma { get; set; }
        public required string Telefone { get; set; }

        public List<Pedido> Pedidos { get; set; } = [];
    }
}