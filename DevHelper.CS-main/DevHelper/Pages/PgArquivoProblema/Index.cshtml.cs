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
    public class IndexModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public IndexModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        public IList<ArquivoProblema> ArquivoProblema { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ArquivoProblema = await _context.ArquivoProblemas
                .Include(a => a.Problema).ToListAsync();
        }
    }
}
