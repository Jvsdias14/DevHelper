using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Model;
using System.Security.Claims;
using DevHelper.Data.Interface;
using DevHelper.Data.Repository;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class CreateModel : PageModel
    {
        private readonly iArquivoProblemaRepositoryAsync ArquivoProblemaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CreateModel> _logger;
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;

        private const string UploadsFolderPath = @"C:\Users\jvsdi\Uploads";

        public CreateModel(iProblemaRepositoryAsync problemaRepositoryAsync, IHttpContextAccessor httpContextAccessor, ILogger<CreateModel> logger, iUsuarioRepositoryAsync usuariorepositoryasync, iArquivoProblemaRepositoryAsync arquivoproblemarepository)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            ArquivoProblemaRepository = arquivoproblemarepository;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;

        [BindProperty]
        public List<IFormFile> UploadedFiles { get; set; } = new List<IFormFile>();

        public Usuario Usuario { get; set; } = default!;

        public ArquivoProblema ArquivoProblema { get; set; } = default!;

        public async Task<IActionResult> OnGet()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToPage("../Index");
            }

            Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(int.Parse(userId));

            Problema = new Problema
            {
                UsuarioId = int.Parse(userId),
                Usuario = Usuario
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Iniciando OnPostAsync");
            _logger.LogInformation($"Número de arquivos recebidos: {UploadedFiles?.Count ?? 0}");

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
                _logger.LogInformation($"Processando arquivo: {file.FileName}, Tamanho: {file.Length}");

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

                    ArquivoProblema = new ArquivoProblema
                    {
                        Problema = Problema,
                        Nome = file.FileName,
                        Referencia = filePath
                    };

                    await ArquivoProblemaRepository.IncluirAsync(ArquivoProblema);
                }
            }

            _logger.LogInformation("Redirecionando para a página de índice");
            return new JsonResult(new { redirectUrl = Url.Page("../Index") });
        }
    }
}
