using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgSolucao
{
    public class CreateModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public CreateModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

      
        public IActionResult OnGet(int? problemaId, int? usuarioId)
        {
        ViewData["ProblemaId"] = new SelectList(_context.Problemas.Where(p => p.Id == problemaId.Value), "Id", "Descricao");
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios.Where(p => p.Id == usuarioId.Value), "Id", "Nome");
            return Page();
        }

        [BindProperty]
        public Solucao Solucao { get; set; } = default!;
        public Problema Problema { get; set; } = default!;

        public Usuario Usuario { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            _context.Solucoes.Add(Solucao);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
