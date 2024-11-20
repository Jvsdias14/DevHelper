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
using DevHelper.Data.Interface;

namespace DevHelper.Razor.Pages.PgCadUsuario
{
    public class DetailsModel : PageModel
    {
        private readonly iUsuarioRepositoryAsync UsuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DetailsModel(iUsuarioRepositoryAsync usuariorepositoryasync, IHttpContextAccessor httpContextAccessor)
        {
            UsuarioRepository = usuariorepositoryasync;
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

            var usuario = await UsuarioRepository.ObterUsuarioComProblemasESolucoesAsync(id.Value);
            if (usuario == null)
            {
                return NotFound();
            }

            Usuario = usuario;
            Problemas = usuario.Problemas;
            Solucoes = usuario.Solucaos;

            var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IsCurrentUser = loggedInUserId == Usuario.Id.ToString();

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("../Index");
        }
    }
}

