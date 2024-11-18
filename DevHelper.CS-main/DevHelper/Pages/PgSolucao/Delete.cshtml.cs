using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgSolucao
{
    public class DeleteModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public DeleteModel(DevHelper.Data.Model.DBdevhelperContext context)
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

            var solucao = await _context.Solucoes.FirstOrDefaultAsync(m => m.Id == id);

            if (solucao == null)
            {
                return NotFound();
            }
            else
            {
                Solucao = solucao;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solucao = await _context.Solucoes.FindAsync(id);
            if (solucao != null)
            {
                Solucao = solucao;
                _context.Solucoes.Remove(Solucao);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
