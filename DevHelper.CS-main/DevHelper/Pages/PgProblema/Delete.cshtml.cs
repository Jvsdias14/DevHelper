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

namespace DevHelper.Razor.Pages.PgProblema
{
    public class DeleteModel : PageModel
    {
        private readonly DBdevhelperContext _context;
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;




        public DeleteModel(iProblemaRepositoryAsync problemaRepositoryAsync, iUsuarioRepositoryAsync usuariorepositoryasync)
        {
            Repository = problemaRepositoryAsync;
            UsuarioRepository = usuariorepositoryasync;
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
                Problema = problema;
                await Repository.ExcluirAsync(Problema);
            }

            return RedirectToPage("../Index");
        }
    }
}
