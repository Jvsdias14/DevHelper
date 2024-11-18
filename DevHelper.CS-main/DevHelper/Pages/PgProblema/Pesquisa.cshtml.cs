using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class PesquisaModel : PageModel
    {
        private readonly DBdevhelperContext _context;

        public PesquisaModel(DBdevhelperContext context)
        {
            _context = context;
        }

        public IList<Problema> Problemas { get; set; }

        public async Task<IActionResult> OnGetAsync(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                ViewData["IsEmpty"] = true;
                Problemas = new List<Problema>();
                return Page();
            }

            ViewData["IsEmpty"] = false;

            var problemas = await _context.Problemas
                .Where(p => p.Nome.Contains(query))
                .ToListAsync();

            foreach (var problema in problemas)
            {
                problema.Usuario = await _context.Usuarios.FindAsync(problema.UsuarioId);
            }

            Problemas = problemas;

            return Page();
        }
    }
}
