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
    public class DetailsModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public DetailsModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

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
    }
}
