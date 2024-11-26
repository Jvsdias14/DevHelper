using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using Microsoft.AspNetCore.Authorization;
using DevHelper.Data.Interfaces;

namespace DevHelper.Razor.Pages.PgProblema
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DBdevhelperContext _context;
        private readonly iProblemaRepositoryAsync Repository;

        public IndexModel(iProblemaRepositoryAsync problemaRepositoryAsync)
        {
            Repository = problemaRepositoryAsync;
        }

        public IList<Problema> Problema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Problema = await Repository.SelecionarProblemaComTudo();
            return Page();
        }
    }
}
