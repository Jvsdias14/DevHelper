using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgArquivoProblema
{
    public class DeleteModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public DeleteModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ArquivoProblema ArquivoProblema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arquivoproblema = await _context.ArquivoProblemas.FirstOrDefaultAsync(m => m.Id == id);

            if (arquivoproblema == null)
            {
                return NotFound();
            }
            else
            {
                ArquivoProblema = arquivoproblema;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arquivoproblema = await _context.ArquivoProblemas.FindAsync(id);
            if (arquivoproblema != null)
            {
                ArquivoProblema = arquivoproblema;
                _context.ArquivoProblemas.Remove(ArquivoProblema);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
