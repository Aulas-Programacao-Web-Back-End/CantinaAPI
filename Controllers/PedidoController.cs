using CantinaAPI.Context;
using CantinaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CantinaAPI.DTOs;

namespace CantinaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoResponseDTO>>> ListarPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Produto)
                .Select(p => new PedidoResponseDTO
                {
                    IdPedido = p.IdPedido,
                    NomeCliente = p.Cliente.Nome,
                    Produto = p.Produto.Descricao,
                    Quantidade = p.Quantidade,
                    ValorTotal = p.ValorTotal,
                    DataPedido = p.DataPedido
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoResponseDTO>> ListarPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Produto)
                .Where(p => p.IdPedido == id)
                .Select(p => new PedidoResponseDTO
                {
                    IdPedido = p.IdPedido,
                    NomeCliente = p.Cliente.Nome,
                    Produto = p.Produto.Descricao,
                    Quantidade = p.Quantidade,
                    ValorTotal = p.ValorTotal,
                    DataPedido = p.DataPedido
                })
                .FirstOrDefaultAsync();

            if (pedido == null) return NotFound();

            return Ok(pedido);
        }

        [HttpPost]
        public async Task<ActionResult<PedidoResponseDTO>> CadastrarPedido(PedidoCreateDTO dto)
        {
            var produto = await _context.Produtos.FindAsync(dto.IdProduto);
            if (produto == null) return BadRequest("Produto não encontrado.");

            var cliente = await _context.Clientes.FindAsync(dto.IdCliente);
            if (cliente == null) return BadRequest("Cliente não encontrado.");

            var pedido = new Pedido
            {
                IdCliente = dto.IdCliente,
                IdProduto = dto.IdProduto,
                Quantidade = dto.Quantidade,
                DataPedido = DateOnly.FromDateTime(DateTime.Today),
                ValorTotal = produto.Preco * dto.Quantidade
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var response = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Produto)
                .Where(p => p.IdPedido == pedido.IdPedido)
                .Select(p => new PedidoResponseDTO
                {
                    IdPedido = p.IdPedido,
                    NomeCliente = p.Cliente.Nome,
                    Produto = p.Produto.Descricao,
                    Quantidade = p.Quantidade,
                    ValorTotal = p.ValorTotal,
                    DataPedido = p.DataPedido
                })
                .FirstAsync();

            return CreatedAtAction(nameof(ListarPedido), new { id = response.IdPedido }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPedido(int id, PedidoCreateDTO dto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            var produto = await _context.Produtos.FindAsync(dto.IdProduto);
            if (produto == null) return BadRequest("Produto não encontrado.");

            var cliente = await _context.Clientes.FindAsync(dto.IdCliente);
            if (cliente == null) return BadRequest("Cliente não encontrado.");

            pedido.IdCliente = dto.IdCliente;
            pedido.IdProduto = dto.IdProduto;
            pedido.Quantidade = dto.Quantidade;
            pedido.ValorTotal = produto.Preco * dto.Quantidade;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}