using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevHelper.Data.Model;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
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

        private const string UploadsFolderPath = @"C:\Users\jvsdi\Uploads";

        public CreateModel(iProblemaRepositoryAsync problemaRepositoryAsync, IHttpContextAccessor httpContextAccessor, ILogger<CreateModel> logger, iUsuarioRepositoryAsync usuariorepositoryasync)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;

        [BindProperty]
        public IFormFileCollection UploadedFiles { get; set; }

        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(userId);
            if (userId == null)
            {
                return RedirectToPage("../Index");
            }

            Problema = new Problema
            {
                UsuarioId = userId,
                Usuario = Usuario
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

            if (Problema.Usuario == null)
            {
                ModelState.AddModelError("Problema.Usuario", "O usuário associado não pôde ser encontrado.");
            }

            ModelState.Remove("Problema.Usuario");

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

            // Salvar arquivos
            foreach (var file in UploadedFiles)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.Combine(UploadsFolderPath, file.FileName);

                    // Verifica se a pasta de uploads existe, caso contrário, cria-a
                    if (!Directory.Exists(UploadsFolderPath))
                    {
                        Directory.CreateDirectory(UploadsFolderPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var arquivoProblema = new ArquivoProblema
                    {
                        ProblemaId = Problema.Id,
                        Nome = file.FileName,
                        Referencia = filePath
                    };

                    await _context.ArquivoProblemas.AddAsync(arquivoProblema);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("../Index");
        }
    }
}
