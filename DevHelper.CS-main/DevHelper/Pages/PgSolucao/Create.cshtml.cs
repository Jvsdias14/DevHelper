using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevHelper.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DevHelper.Data.Interface;
using System.Security.Claims;
using DevHelper.Data.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DevHelper.Razor.Pages.PgSolucao
{
    public class CreateModel : PageModel
    {
        private readonly iSolucaoRepositoryAsync SolucaoRepository;
        private readonly ILogger<CreateModel> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly iProblemaRepositoryAsync ProblemaRepository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;

        public CreateModel(iSolucaoRepositoryAsync SolucaoRepositoryAsync, ILogger<CreateModel> logger, IHttpContextAccessor httpContextAccessor, iProblemaRepositoryAsync problemaRepository, iUsuarioRepositoryAsync usuarioRepository)
        {
            SolucaoRepository = SolucaoRepositoryAsync;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            ProblemaRepository = problemaRepository;
            UsuarioRepository = usuarioRepository;
        }

        [BindProperty]
        public Solucao Solucao { get; set; } = default!;

        public Problema Problema { get; set; } = default!;

        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? problemaId)
        {
            if (problemaId == null)
            {
                return NotFound();
            }

            var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (loggedInUserId == null)
            {
                _logger.LogWarning("Usuário não autenticado. Redirecionando para Index.");
                return RedirectToPage("../Index");
            }

            Problema = await ProblemaRepository.SelecionaPelaChaveAsync(problemaId.Value);
            if (Problema == null)
            {
                _logger.LogWarning("Problema não encontrado. Redirecionando para Index.");
                return RedirectToPage("../Index");
            }

            Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(int.Parse(loggedInUserId));
            if (Usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado. Redirecionando para Index.");
                return RedirectToPage("../Index");
            }

            Solucao = new Solucao
            {
                ProblemaId = problemaId.Value,
                UsuarioId = int.Parse(loggedInUserId),
                Usuario = Usuario,
                Problema = Problema
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Solucao.Usuario");
            ModelState.Remove("Solucao.Problema");


            if (!ModelState.IsValid)
            {
                return Page();
            }

            await SolucaoRepository.IncluirAsync(Solucao);

            return RedirectToPage("../Index");
        }
    }
}
