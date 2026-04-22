using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CantinaAPI.Context;
using CantinaAPI.Models;
using CantinaAPI.DTOs;

namespace CantinaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponseDTO>>> ListarClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteResponseDTO
                {
                    IdCliente = c.IdCliente,
                    Nome = c.Nome,
                    Turma = c.Turma,
                    Telefone = c.Telefone
                })
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponseDTO>> ListarCliente(int id)
        {
            var cliente = await _context.Clientes
                .Where(c => c.IdCliente == id)
                .Select(c => new ClienteResponseDTO
                {
                    IdCliente = c.IdCliente,
                    Nome = c.Nome,
                    Turma = c.Turma,
                    Telefone = c.Telefone
                })
                .FirstOrDefaultAsync();

            if (cliente == null) return NotFound();

            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteResponseDTO>> CadastrarCliente(ClienteCreateDTO dto)
        {
            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Turma = dto.Turma,
                Telefone = dto.Telefone
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var response = new ClienteResponseDTO
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome,
                Turma = cliente.Turma,
                Telefone = cliente.Telefone
            };

            return CreatedAtAction(nameof(ListarCliente), new { id = response.IdCliente }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCliente(int id, ClienteCreateDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            cliente.Nome = dto.Nome;
            cliente.Turma = dto.Turma;
            cliente.Telefone = dto.Telefone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/relatorio")]
        public async Task<ActionResult<ClienteRelatorioDTO>> GerarRelatorio(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Pedidos)
                    .ThenInclude(p => p.Produto)
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null) return NotFound();

            // Produto mais pedido — soma quantidade por produto e pega o maior
            var produtoMaisPedido = cliente.Pedidos
                .GroupBy(p => p.Produto.Descricao)
                .OrderByDescending(g => g.Sum(p => p.Quantidade))
                .Select(g => g.Key)
                .FirstOrDefault() ?? "Nenhum pedido";

            var relatorio = new ClienteRelatorioDTO
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome,
                Turma = cliente.Turma,
                Telefone = cliente.Telefone,

                TotalPedidos = cliente.Pedidos.Count,
                TotalGasto = (decimal)cliente.Pedidos.Sum(p => p.ValorTotal),
                ProdutoMaisPedido = produtoMaisPedido,

                Pedidos = cliente.Pedidos
                    .OrderByDescending(p => p.DataPedido)
                    .Select(p => new PedidoResumoDTO
                    {
                        IdPedido = p.IdPedido,
                        Produto = p.Produto.Descricao,
                        Quantidade = p.Quantidade,
                        ValorTotal = (decimal)p.ValorTotal,
                        DataPedido = p.DataPedido
                    }).ToList()
            };

            return Ok(relatorio);
        }
    }
}