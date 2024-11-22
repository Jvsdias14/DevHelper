using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevHelper.Data.Interfaces;

namespace DevHelper.Data.Controladoras
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolucoesController : ControllerBase
    {
        private readonly iProblemaRepositoryAsync _problemaRepository;

        public SolucoesController(iProblemaRepositoryAsync problemaRepository)
        {
            _problemaRepository = problemaRepository;
        }

        [HttpGet("{problemaId}/solucoes/restantes")]
        public async Task<IActionResult> GetSolucoesRestantes(int problemaId, int carregadas)
        {
            var solucoes = await _problemaRepository.BuscarSolucoesRestantesAsync(problemaId, carregadas);
            return Ok(solucoes);
        }
    }
}
