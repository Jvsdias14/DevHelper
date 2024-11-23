using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevHelper.Data.Model;
using DevHelper.Data.Interface;
using DevHelper.Data.Interfaces;

namespace DevHelper.Razor.Pages.PgSolucao
{
    public class EditModel : PageModel
    {
        private readonly iSolucaoRepositoryAsync SolucaoRepository;
        private readonly iProblemaRepositoryAsync ProblemaRepository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;

        public EditModel(iSolucaoRepositoryAsync solucaoRepository, iProblemaRepositoryAsync problemaRepository, iUsuarioRepositoryAsync usuarioRepository)
        {
            SolucaoRepository = solucaoRepository;
            ProblemaRepository = problemaRepository;
            UsuarioRepository = usuarioRepository;
        }

        [BindProperty]
        public Solucao Solucao { get; set; } = default!;
        public Problema Problema { get; set; } = default!;
        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solucao = await SolucaoRepository.SelecionaPelaChaveAsync(id.Value);
            if (solucao == null)
            {
                return NotFound();
            }

            Solucao = solucao;
            Problema = await ProblemaRepository.SelecionaPelaChaveAsync(solucao.ProblemaId);
            Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(solucao.UsuarioId);

            if (Problema == null || Usuario == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            var solucaoToUpdate = await SolucaoRepository.SelecionaPelaChaveAsync(id);
            if (solucaoToUpdate == null)
            {
                return NotFound();
            }

            solucaoToUpdate.Descricao = Solucao.Descricao;

            await SolucaoRepository.AlterarAsync(solucaoToUpdate);

            //return RedirectToPage("./Details", new { id = solucaoToUpdate.Id });
            return RedirectToPage("../Index");
        }
    }
}
