namespace CantinaAPI.DTOs
{
    public class PedidoResumoDTO
    {
        public int IdPedido { get; set; }
        public string Produto { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public DateOnly DataPedido { get; set; }
    }

    public class ClienteRelatorioDTO
    {
        // Dados do cliente
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Turma { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

        // Resumo financeiro
        public int TotalPedidos { get; set; }
        public decimal TotalGasto { get; set; }
        public string ProdutoMaisPedido { get; set; } = string.Empty;

        // Lista completa de pedidos
        public List<PedidoResumoDTO> Pedidos { get; set; } = new();
    }
}