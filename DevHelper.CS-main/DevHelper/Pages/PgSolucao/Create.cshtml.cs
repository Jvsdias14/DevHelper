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

namespace DevHelper.Razor.Pages.PgSolucao
{
    public class CreateModel : PageModel
    {
        private readonly DBdevhelperContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(DBdevhelperContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Solucao Solucao { get; set; } = default!;

        public Problema Problema { get; set; } = default!;

        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? problemaId, int? usuarioId)
        {
            // Adicionando logs para depuração
            _logger.LogInformation("Iniciando OnGetAsync");
            _logger.LogInformation($"problemaId: {problemaId}, usuarioId: {usuarioId}");

            if (problemaId == null || usuarioId == null)
            {
                _logger.LogWarning("ProblemaId ou UsuarioId nulos. Redirecionando para Index.");
                return RedirectToPage("../Index");
            }

            Problema = await _context.Problemas.FindAsync(problemaId.Value);
            Usuario = await _context.Usuarios.FindAsync(usuarioId.Value);

            if (Problema == null || Usuario == null)
            {
                _logger.LogWarning("Problema ou Usuario não encontrado. Redirecionando para Index.");
                return RedirectToPage("../Index");
            }

            Solucao = new Solucao
            {
                ProblemaId = Problema.Id,
                UsuarioId = Usuario.Id
            };

            ViewData["ProblemaId"] = new SelectList(_context.Problemas, "Id", "Descricao", Solucao.ProblemaId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", Solucao.UsuarioId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Atribuir os objetos Problema e Usuario antes da validação do ModelState
            Solucao.Problema = await _context.Problemas.FindAsync(Solucao.ProblemaId);
            Solucao.Usuario = await _context.Usuarios.FindAsync(Solucao.UsuarioId);

            if (Solucao.Problema == null)
            {
                ModelState.AddModelError("Solucao.ProblemaId", "Problema inválido.");
            }

            if (Solucao.Usuario == null)
            {
                ModelState.AddModelError("Solucao.UsuarioId", "Usuário inválido.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProblemaId"] = new SelectList(_context.Problemas, "Id", "Descricao", Solucao.ProblemaId);
                ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", Solucao.UsuarioId);
                return Page();
            }

            _context.Solucoes.Add(Solucao);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
