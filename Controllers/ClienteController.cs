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
    }
}