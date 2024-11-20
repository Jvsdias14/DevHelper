using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using DevHelper.Data.Interface;

namespace DevHelper.Razor.Pages.PgCadUsuario
{
    public class EditModel : PageModel
    {
        private readonly iUsuarioRepositoryAsync UsuarioRepository;

        public EditModel(iUsuarioRepositoryAsync usuariorepositoryasync)
        {
            UsuarioRepository = usuariorepositoryasync;
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
            if (usuario == null)
            {
                return NotFound();
            }
            Usuario = usuario;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await UsuarioRepository.AlterarAsync(Usuario);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UsuarioExistsAsync(Usuario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Index");
        }

        private async Task<bool> UsuarioExistsAsync(int id)
        {
            return await UsuarioRepository.SelecionaPelaChaveAsync(id) != null;
        }
    }
}
