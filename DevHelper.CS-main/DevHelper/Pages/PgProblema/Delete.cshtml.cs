using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Interface;
using DevHelper.Data.Repository;
using System.IO;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class DeleteModel : PageModel
    {
        private readonly iArquivoProblemaRepositoryAsync ArquivoProblemaRepository;
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;
        private readonly iSolucaoRepositoryAsync SolucaoRepository;

        private const string UploadsFolderPath = @"C:\Users\jvsdi\Uploads";

        public DeleteModel(iProblemaRepositoryAsync problemaRepositoryAsync, iUsuarioRepositoryAsync usuariorepositoryasync, iArquivoProblemaRepositoryAsync arquivoproblemarepository, iSolucaoRepositoryAsync solucaoRepository)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
            ArquivoProblemaRepository = arquivoproblemarepository;
            SolucaoRepository = solucaoRepository;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema = await Repository.SelecionaPelaChaveAsync(id.Value);

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema = await Repository.SelecionaPelaChaveAsync(id.Value);
            if (problema != null)
            {
                // Carregar os ArquivoProblemas relacionados
                Problema = await Repository.ProblemaCompletoAsync(problema.Id);
                var arquivos = Problema.ArquivoProblemas.ToList();
                var solucoes = Problema.Solucaos.ToList();

                // Deletar cada arquivo fisicamente e do banco de dados
                foreach (var arquivo in arquivos)
                {
                    var filePath = Path.Combine(UploadsFolderPath, arquivo.Nome);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    await ArquivoProblemaRepository.ExcluirAsync(arquivo);
                }

                foreach (var solucao in solucoes)
                {
                    await SolucaoRepository.ExcluirAsync(solucao);
                }

                Problema = problema;
                await Repository.ExcluirAsync(Problema);
            }

            return RedirectToPage("../Index");
        }
    }
}
