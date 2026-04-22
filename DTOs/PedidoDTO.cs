using System.ComponentModel.DataAnnotations;

namespace CantinaAPI.DTOs
{
    public class PedidoCreateDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "IdCliente inválido")]
        public int IdCliente { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "IdProduto inválido")]
        public int IdProduto { get; set; }

        [Range(1, 100, ErrorMessage = "Quantidade deve ser entre 1 e 100")]
        public int Quantidade { get; set; }
    }

    public class PedidoResponseDTO
    {
        public int IdPedido { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
        public string Produto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public DateOnly DataPedido { get; set; }
    }
}