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
    public class IndexModel : PageModel
    {
        private readonly iUsuarioRepositoryAsync UsuarioRepository;
        public IndexModel(iUsuarioRepositoryAsync usuariorepositoryasync)
        {
            UsuarioRepository = usuariorepositoryasync;
        }

        public IList<Usuario> Usuario { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Usuario = await UsuarioRepository.SelecionTodosAsync();
        }
    }
}
