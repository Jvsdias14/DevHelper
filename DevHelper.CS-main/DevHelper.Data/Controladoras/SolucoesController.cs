using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevHelper.Data.Model;

namespace DevHelper.Data.Controladoras
{
    [Route("Solucoes")]
    [ApiController]
    public class SolucoesController : ControllerBase
    {
        private readonly DBdevhelperContext _context;

        public SolucoesController(DBdevhelperContext context)
        {
            _context = context;
        }

        [HttpPost("Like/{id}")]
        public async Task<IActionResult> Like(int id)
        {
            var solucao = await _context.Solucoes.FindAsync(id);
            if (solucao == null)
            {
                return NotFound();
            }

            solucao.LikeCount++;
            await _context.SaveChangesAsync();

            return Ok(new { likeCount = solucao.LikeCount });
        }

        [HttpPost("Dislike/{id}")]
        public async Task<IActionResult> Dislike(int id)
        {
            var solucao = await _context.Solucoes.FindAsync(id);
            if (solucao == null)
            {
                return NotFound();
            }

            solucao.DislikeCount++;
            await _context.SaveChangesAsync();

            return Ok(new { dislikeCount = solucao.DislikeCount });
        }
    }
}
