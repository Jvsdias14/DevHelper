using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevHelper.Data.Model;
using DevHelper.Data.Interfaces;
using DevHelper.Data.Interface;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class DetailsModel : PageModel
    {
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;
        private readonly iSolucaoRepositoryAsync SolucaoRepository;
        private readonly iArquivoProblemaRepositoryAsync ArquivoProblemaRepository;

        private const string UploadsFolderPath = @"C:\Users\jvsdi\Uploads";

        public DetailsModel(iProblemaRepositoryAsync problemaRepositoryAsync, iUsuarioRepositoryAsync usuariorepositoryasync, iSolucaoRepositoryAsync solucaoRepository, iArquivoProblemaRepositoryAsync arquivoproblemarepository)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
            SolucaoRepository = solucaoRepository;
            ArquivoProblemaRepository = arquivoproblemarepository;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;
        public Usuario Usuario { get; set; } = default!;
        [BindProperty]
        public List<IFormFile> UploadedFiles { get; set; } = new List<IFormFile>();
        [BindProperty]
        public string FilesToRemove { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema = await Repository.ProblemaCompletoAsync(id.Value);
            foreach (var solucao in problema.Solucaos)
            {
                solucao.Usuario = await UsuarioRepository.SelecionaPelaChaveAsync(solucao.UsuarioId);
            }

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

            var problemaToUpdate = await Repository.ProblemaCompletoAsync(id);
            if (problemaToUpdate == null)
            {
                return NotFound();
            }

            // Atualizar a descrição com o novo valor vindo do front-end
            problemaToUpdate.Descricao = Problema.Descricao;
            //Problema = problemaToUpdate;

            // Processar arquivos removidos
            if (!string.IsNullOrEmpty(FilesToRemove))
            {
                var filesToRemove = JsonConvert.DeserializeObject<List<int>>(FilesToRemove);
                foreach (var fileId in filesToRemove)
                {
                    var arquivo = await ArquivoProblemaRepository.SelecionaPelaChaveAsync(fileId);
                    if (arquivo != null)
                    {
                        var filePath = Path.Combine(UploadsFolderPath, arquivo.Nome);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        await ArquivoProblemaRepository.ExcluirAsync(arquivo);
                    }
                }
            }

            // Processar novos arquivos enviados
            if (UploadedFiles != null && UploadedFiles.Count > 0)
            {
                foreach (var file in UploadedFiles)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(UploadsFolderPath, file.FileName);

                        if (!Directory.Exists(UploadsFolderPath))
                        {
                            Directory.CreateDirectory(UploadsFolderPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var arquivoProblema = new ArquivoProblema
                        {
                            ProblemaId = problemaToUpdate.Id,
                            Nome = file.FileName,
                            Referencia = filePath
                        };

                        await ArquivoProblemaRepository.IncluirAsync(arquivoProblema);
                    }
                }
            }
            ModelState.Remove("FilesToRemove");
            ModelState.Remove("Problema.Usuario");
            ModelState.Remove("Problema.Nome");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Salvar as alterações no banco de dados
            await Repository.AlterarAsync(problemaToUpdate);

            return RedirectToPage("../Index");
        }
    }
}
