namespace CantinaAPI.DTOs
{
    public class ProdutoCreateDTO
    {
        public string Descricao { get; set; } = string.Empty;
        public double Preco { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }

    public class ProdutoResponseDTO
    {
        public int IdProduto { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public double Preco { get; set; }
        public string Categoria { get; set; } = string.Empty;
    }
}