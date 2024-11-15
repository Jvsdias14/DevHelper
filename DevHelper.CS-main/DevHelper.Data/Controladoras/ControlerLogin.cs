using DevHelper.Data.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevHelper.Data.Controladoras
{
    public class AccountController : Controller
    {
        private readonly AuthenticationService _authService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly DBdevhelperContext _context;

        public AccountController(AuthenticationService authService, IPasswordHasher<Usuario> passwordHasher, DBdevhelperContext context)
        {
            _authService = authService;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        // Login
        public async Task<IActionResult> Login(string email, string password)
        {
            // Buscar o usuário do banco de dados
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // Se não encontrar o usuário, retornar erro de login inválido
                return View();
            }

            // Verificar se a senha fornecida corresponde ao hash da senha armazenado
            var result = _passwordHasher.VerifyHashedPassword(user, user.Senha, password);
            if (result == PasswordVerificationResult.Failed)
            {
                // Se a senha não for válida, retornar erro
                return View();
            }

            // Criação do cookie de autenticação
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email),
                // Outros claims necessários
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Tornar o login persistente (se necessário)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
