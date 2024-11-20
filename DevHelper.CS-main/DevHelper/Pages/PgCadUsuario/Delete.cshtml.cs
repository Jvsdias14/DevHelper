using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using DevHelper.Data.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DevHelper.Razor.Pages.PgCadUsuario
{
    public class DeleteModel : PageModel
    {
        private readonly iUsuarioRepositoryAsync UsuarioRepository;
        private readonly IHttpContextAccessor _httpcontextAccessor;

        public DeleteModel( iUsuarioRepositoryAsync usuariorepositoryasync, IHttpContextAccessor httpcontextAccessor)
        {
            UsuarioRepository = usuariorepositoryasync;
            _httpcontextAccessor = httpcontextAccessor;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await UsuarioRepository.SelecionaPelaChaveAsync(id.Value);
            var userId = _httpcontextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuario == null)
            {
                return NotFound();
            }

            if (userId == null || usuario.Id != int.Parse(userId))
            {
                return Forbid();
            }

            Usuario = usuario;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await UsuarioRepository.SelecionaPelaChaveAsync(id.Value);
            var userId = _httpcontextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuario == null)
            {
                return NotFound();
            }

            if (userId == null || usuario.Id != int.Parse(userId))
            {
                return Forbid();
            }

            await UsuarioRepository.ExcluirAsync(usuario);

            // Limpar sessões e cookies após a exclusão
            LimparDadosUsuario();

            return RedirectToPage("../Index");
        }

        private void LimparDadosUsuario()
        {
            // Limpar cookies
            var cookies = _httpcontextAccessor.HttpContext.Request.Cookies.Keys.ToList();
            foreach (var cookie in cookies)
            {
                _httpcontextAccessor.HttpContext.Response.Cookies.Delete(cookie);
            }

            // Limpar sessões
            _httpcontextAccessor.HttpContext.Session.Clear();
        }
    }
}
