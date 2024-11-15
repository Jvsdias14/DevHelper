using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgSolucao
{
    public class EditModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public EditModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Solucao Solucao { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solucao =  await _context.Solucoes.FirstOrDefaultAsync(m => m.Id == id);
            if (solucao == null)
            {
                return NotFound();
            }
            Solucao = solucao;
           ViewData["ProblemaId"] = new SelectList(_context.Problemas, "Id", "Descricao");
           ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Solucao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolucaoExists(Solucao.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SolucaoExists(int id)
        {
            return _context.Solucoes.Any(e => e.Id == id);
        }
    }
}
