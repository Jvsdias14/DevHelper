using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class CreateModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public CreateModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome");
            return Page();
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            _context.Problemas.Add(Problema);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
