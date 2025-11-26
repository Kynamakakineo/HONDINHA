using AutoManagerApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AutoManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogisticaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LogisticaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("mover")]
        public async Task<IActionResult> MoverCarro(int carroId, int novaConcessionariaId)
        {
            // 1. Encontrar o carro
            var carro = await _context.Carros.FindAsync(carroId);
            if (carro == null)
            {
                return NotFound($"Carro com ID {carroId} não encontrado.");
            }

            // 2. Verificar se a concessionária de destino existe
            var novaConcessionaria = await _context.Concessionarias.FindAsync(novaConcessionariaId);
            if (novaConcessionaria == null)
            {
                return BadRequest($"Concessionária com ID {novaConcessionariaId} não existe.");
            }

            if (carro.ConcessionariaId == novaConcessionariaId)
            {
                return BadRequest("O carro já está nesta concessionária.");
            }

            // 3. Atualizar o ID da concessionária no carro
            carro.ConcessionariaId = novaConcessionariaId;
            
            // 4. Salvar a mudança no banco de dados
            _context.Carros.Update(carro);
            await _context.SaveChangesAsync();

            return Ok($"Carro {carro.Modelo} movido com sucesso para: {novaConcessionaria.Nome}");
        }
    }
}