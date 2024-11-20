using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Interface;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class PesquisaModel : PageModel
    {
        private readonly DBdevhelperContext _context;
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;

        public PesquisaModel(iProblemaRepositoryAsync problemaRepositoryAsync, iUsuarioRepositoryAsync usuariorepositoryasync)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
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

            Problemas = await Repository.Pesquisar(query);

            // Carregar os usuários associados aos problemas
            foreach (var problema in Problemas)
            {
                problema.Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(problema.UsuarioId);
            }

            return Page();
        }
    }
}
