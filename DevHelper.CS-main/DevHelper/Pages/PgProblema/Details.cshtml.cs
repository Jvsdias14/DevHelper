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
    public class DetailsModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public DetailsModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        public Problema Problema { get; set; } = default!;

        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema = await _context.Problemas.FirstOrDefaultAsync(m => m.Id == id);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == problema.UsuarioId);


            if (problema == null)
            {
                return NotFound();
            }
            else
            {
                Problema = problema;
                Usuario = usuario;
            }
            return Page();
        }
    }
}
