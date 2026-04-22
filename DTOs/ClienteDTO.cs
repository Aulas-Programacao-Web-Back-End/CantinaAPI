using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CantinaAPI.DTOs
{
    public class ClienteCreateDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Turma é obrigatória")]
        public string Turma { get; set; }

        [Phone(ErrorMessage = "Telefone inválido")]
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