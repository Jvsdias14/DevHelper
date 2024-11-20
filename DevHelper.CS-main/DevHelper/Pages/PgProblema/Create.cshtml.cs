using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevHelper.Data.Model;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using DevHelper.Data.Repositories;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Interface;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class CreateModel : PageModel
    {
        private readonly DBdevhelperContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateModel> _logger;
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;



        public CreateModel(iProblemaRepositoryAsync problemaRepositoryAsync, IHttpContextAccessor httpContextAccessor, ILogger<CreateModel> logger, iUsuarioRepositoryAsync usuariorepositoryasync)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;

        public IActionResult OnGet()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("../Index");
            }

            Problema = new Problema
            {
                UsuarioId = int.Parse(userId)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Iniciando OnPostAsync");

            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("Usuário não autenticado");
                return RedirectToPage("../Index");
            }

            Problema.UsuarioId = int.Parse(userId);
            Problema.Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(Problema.UsuarioId);

            // Verificação manual da propriedade de navegação Usuario
            if (Problema.Usuario == null)
            {
                ModelState.AddModelError("Problema.Usuario", "O usuário associado não pôde ser encontrado.");
            }

            // Remover a propriedade de navegação do ModelState para evitar validação desnecessária
            ModelState.Remove("Problema.Usuario");

            // Log para verificar o estado do problema antes da validação
            _logger.LogInformation($"Pre-Validation - Problema: {Problema.Nome}, Descricao: {Problema.Descricao}, UsuarioId: {Problema.UsuarioId}, Usuario: {Problema.Usuario?.Nome ?? "null"}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning($"Erro: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            await Repository.IncluirAsync(Problema);

            _logger.LogInformation("Problema cadastrado com sucesso");

            return RedirectToPage("../Index");
        }
    }
}
