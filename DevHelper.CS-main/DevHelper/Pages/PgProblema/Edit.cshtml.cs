using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using DevHelper.Data.Interface;
using DevHelper.Data.Interfaces;

namespace DevHelper.Razor.Pages.PgProblema
{
    public class EditModel : PageModel
    {
        private readonly DBdevhelperContext _context;
        private readonly iProblemaRepositoryAsync Repository;
        private readonly iUsuarioRepositoryAsync UsuarioRepository;

        public EditModel(iProblemaRepositoryAsync problemaRepositoryAsync)
        {
            Repository = problemaRepositoryAsync;
        }

        [BindProperty]
        public Problema Problema { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problema =  await Repository.SelecionaPelaChaveAsync(id.Value);
            if (problema == null)
            {
                return NotFound();
            }
            Problema = problema;
           ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await Repository.AlterarAsync(Problema);

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!ProblemaExists(Problema.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("../Index");
        }

        //        private bool ProblemaExists(int id)
        //        {
        //            return _context.Problemas.Any(e => e.Id == id);
        //        }
    }
}
