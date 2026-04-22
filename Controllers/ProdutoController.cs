using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CantinaAPI.Models;
using CantinaAPI.Context;
using CantinaAPI.DTOs;

namespace CantinaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDTO>>> ListarProdutos()
        {
            var produtos = await _context.Produtos
                .Select(p => new ProdutoResponseDTO
                {
                    IdProduto = p.IdProduto,
                    Descricao = p.Descricao,
                    Preco = (decimal)p.Preco,
                    Categoria = p.Categoria
                })
                .ToListAsync();

            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> ListarProduto(int id)
        {
            var produto = await _context.Produtos
                .Where(p => p.IdProduto == id)
                .Select(p => new ProdutoResponseDTO
                {
                    IdProduto = p.IdProduto,
                    Descricao = p.Descricao,
                    Preco = (decimal)p.Preco,
                    Categoria = p.Categoria
                })
                .FirstOrDefaultAsync();

            if (produto == null) return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDTO>> CadastrarProduto(ProdutoCreateDTO dto)
        {
            var produto = new Produto
            {
                Descricao = dto.Descricao,
                Preco = (double)dto.Preco,
                Categoria = dto.Categoria.ToString()
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            var response = new ProdutoResponseDTO
            {
                IdProduto = produto.IdProduto,
                Descricao = produto.Descricao,
                Preco = (decimal)produto.Preco,
                Categoria = produto.Categoria
            };

            return CreatedAtAction(nameof(ListarProduto), new { id = response.IdProduto }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarProduto(int id, ProdutoCreateDTO dto)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            produto.Descricao = dto.Descricao;
            produto.Preco = (double)dto.Preco;
            produto.Categoria = dto.Categoria.ToString();

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoverProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) return NotFound();

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}