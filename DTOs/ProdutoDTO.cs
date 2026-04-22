using System.ComponentModel.DataAnnotations;

namespace CantinaAPI.DTOs
{
    public enum CategoriaProduto
    {
        Lanche,
        Bebida,
        Sobremesa,
        Outros
    }

    public class ProdutoCreateDTO
    {
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Descrição deve ter entre 2 e 100 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Range(0.01, 9999.99, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatória")]
        [EnumDataType(typeof(CategoriaProduto), ErrorMessage = "Categoria inválida")]
        public CategoriaProduto Categoria { get; set; }
    }

    public class ProdutoResponseDTO
    {
        public int IdProduto { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Categoria { get; set; } = string.Empty; // string no response é ok, mais legível pro frontend
    }
}