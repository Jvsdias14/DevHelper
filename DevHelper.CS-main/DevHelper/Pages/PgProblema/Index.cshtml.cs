using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using Microsoft.AspNetCore.Authorization;

namespace DevHelper.Razor.Pages.PgProblema
{
    [Authorize] // Adicione esta linha para proteger a página
    public class IndexModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public IndexModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        public IList<Problema> Problema { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Problema = await _context.Problemas
                .Include(p => p.Usuario).ToListAsync();
        }
    }
}
