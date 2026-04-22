using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantinaAPI.DTOs
{
    public class ClienteCreateDTO
    {
        public string Nome { get; set; }
        public string Turma { get; set; }
        public string Telefone { get; set; }
    }

    public class ClienteResponseDTO
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Turma { get; set; }
        public string Telefone { get; set; }
    }
}