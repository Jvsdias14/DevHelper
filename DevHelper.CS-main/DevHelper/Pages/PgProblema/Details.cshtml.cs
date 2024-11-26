using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevHelper.Data.Model;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Interface;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class DetailsModel : PageModel
    {
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;
        private readonly iSolucaoRepositoryAsync SolucaoRepository;

        public DetailsModel(iProblemaRepositoryAsync problemaRepositoryAsync, iUsuarioRepositoryAsync usuariorepositoryasync, iSolucaoRepositoryAsync solucaoRepository)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
            SolucaoRepository = solucaoRepository;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;
        public Usuario Usuario { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema = await Repository.ProblemaCompletoAsync(id.Value);
            //var usuario = await UsuarioRepository.SelecionaPelaChaveAsync(problema.UsuarioId);
            //var solucoes = await SolucaoRepository.SeleccionarSolucoesPorProblema(problema.Id);

            //foreach (var solucao in solucoes)
            //{
            //    solucao.Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(solucao.UsuarioId);
            //}

            if (problema == null)
            {
                return NotFound();
            }
            else
            {
                Problema = problema;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var problemaToUpdate = await Repository.SelecionaPelaChaveAsync(id);
            if (problemaToUpdate == null)
            {
                return NotFound();
            }

            // Atualizar a descrição com o novo valor vindo do front-end
            problemaToUpdate.Descricao = Problema.Descricao;

            // Salvar as alterações no banco de dados
            await Repository.AlterarAsync(problemaToUpdate);

            return RedirectToPage("../Index");
        }
    }
}
