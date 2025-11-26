using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoManagerApi.Data;
using AutoManagerApi.Models;

namespace AutoManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcessionariasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConcessionariasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Concessionarias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Concessionaria>>> GetConcessionarias()
        {
            return await _context.Concessionarias
                .Include(c => c.Carros)
                .ToListAsync();
        }

        // GET: api/Concessionarias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Concessionaria>> GetConcessionaria(int id)
        {
            var c = await _context.Concessionarias
                .Include(c => c.Carros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (c == null) return NotFound();
            return c;
        }

        // POST: api/Concessionarias
        [HttpPost]
        public async Task<ActionResult<Concessionaria>> PostConcessionaria(Concessionaria concessionaria)
        {
            _context.Concessionarias.Add(concessionaria);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetConcessionaria), new { id = concessionaria.Id }, concessionaria);
        }

        // PUT: api/Concessionarias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConcessionaria(int id, Concessionaria concessionaria)
        {
            if (id != concessionaria.Id) return BadRequest();

            _context.Entry(concessionaria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Concessionarias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcessionaria(int id)
        {
            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria == null) return NotFound();

            _context.Concessionarias.Remove(concessionaria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST para vincular carro
        [HttpPost("{concessionariaId}/adicionar-carro/{carroId}")]
        public async Task<IActionResult> AdicionarCarro(int concessionariaId, int carroId)
        {
            var concessionaria = await _context.Concessionarias.FindAsync(concessionariaId);
            if (concessionaria == null) return NotFound("Concessionária não encontrada.");

            var carro = await _context.Carros.FindAsync(carroId);
            if (carro == null) return NotFound("Carro não encontrado.");

            carro.ConcessionariaId = concessionariaId;
            await _context.SaveChangesAsync();

            return Ok("Carro vinculado à concessionária.");
        }
    }
}
