using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevHelper.Data.Model;
using Microsoft.AspNetCore.Identity; // Adicione esta linha
using Microsoft.EntityFrameworkCore;

namespace DevHelper.Razor.Pages.PgCadUsuario
{
    public class CreateModel : PageModel
    {
        private readonly DBdevhelperContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher; // Adicione esta linha

        public CreateModel(DBdevhelperContext context, IPasswordHasher<Usuario> passwordHasher) // Atualize o construtor
        {
            _context = context;
            _passwordHasher = passwordHasher; // Inicialize o passwordHasher
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verificar se o e-mail já está cadastrado
            var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == Usuario.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Usuario.Email", "Este e-mail já está em uso.");
                return Page();
            }

            // Criptografar a senha
            Usuario.Senha = _passwordHasher.HashPassword(Usuario, Usuario.Senha);

            _context.Usuarios.Add(Usuario);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
