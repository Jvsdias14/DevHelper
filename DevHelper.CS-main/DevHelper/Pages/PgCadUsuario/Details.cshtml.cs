using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using DevHelper.Data.Model;
using System.Security.Claims;

namespace DevHelper.Razor.Pages.PgCadUsuario
{
    public class DetailsModel : PageModel
    {
        private readonly DevHelper.Data.Model.DBdevhelperContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(DBdevhelperContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Usuario Usuario { get; set; } = default!;
        public bool IsCurrentUser { get; set; }
        public ICollection<Problema> Problemas { get; set; } = default!;
        public ICollection<Solucao> Solucoes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Problemas)
                .Include(u => u.Solucaos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                Usuario = usuario;
                Problemas = usuario.Problemas;
                Solucoes = usuario.Solucaos;

                var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                IsCurrentUser = loggedInUserId == Usuario.Id.ToString();
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
