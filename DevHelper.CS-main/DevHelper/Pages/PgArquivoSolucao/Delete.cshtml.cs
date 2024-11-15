using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgArquivoSolucao
{
    public class DeleteModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public DeleteModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ArquivoSolucao ArquivoSolucao { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arquivosolucao = await _context.ArquivoSolucoes.FirstOrDefaultAsync(m => m.Id == id);

            if (arquivosolucao == null)
            {
                return NotFound();
            }
            else
            {
                ArquivoSolucao = arquivosolucao;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var arquivosolucao = await _context.ArquivoSolucoes.FindAsync(id);
            if (arquivosolucao != null)
            {
                ArquivoSolucao = arquivosolucao;
                _context.ArquivoSolucoes.Remove(ArquivoSolucao);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
