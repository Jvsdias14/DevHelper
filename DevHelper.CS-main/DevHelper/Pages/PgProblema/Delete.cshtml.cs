using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class DeleteModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public DeleteModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema = await _context.Problemas.FirstOrDefaultAsync(m => m.Id == id);

            if (problema == null)
            {
                return NotFound();
            }
            else
            {
                Problema = problema;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema = await _context.Problemas.FindAsync(id);
            if (problema != null)
            {
                Problema = problema;
                _context.Problemas.Remove(Problema);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
