using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DevHelper.Data.Model;

namespace DevHelper.Razor.Pages.PgCadUsuario
{
    public class DetailsModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;

        public DetailsModel(DevHelper.Data.Model.DBdevhelperContext context)
        {
            _context = context;
        }

        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                Usuario = usuario;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("../Index");
        }
    }
}
