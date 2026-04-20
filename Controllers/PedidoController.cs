using CantinaAPI.Context;
using CantinaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Pedido>>> ListarPedidos()
        {
            return Ok(await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Produto)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> ListarPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Produto)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPedido(Pedido pedido)
        {
            var produto = await _context.Produtos.FindAsync(pedido.IdProduto);
            if(produto == null) return BadRequest("Produto não encontrado.");
            
            var cliente = await _context.Clientes.FindAsync(pedido.IdCliente);
            if(cliente == null) return BadRequest("Cliente não encontrado.");

            pedido.DataPedido = DateOnly.FromDateTime(DateTime.Today);
            pedido.ValorTotal = produto.Preco * pedido.Quantidade;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ListarPedido), new { id = pedido.IdPedido }, pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPedido(int id, Pedido pedido)
        {
            if(id != pedido.IdPedido) return BadRequest();

            var produto = await _context.Produtos.FindAsync(pedido.IdProduto);
            if(produto == null) return BadRequest("Produto não encontrado.");
            
            pedido.ValorTotal = produto.Preco * pedido.Quantidade;

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pedidos.Any(p => p.IdPedido == id)) return NotFound();
                throw;
            }
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if(pedido == null) return NotFound();
            
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}