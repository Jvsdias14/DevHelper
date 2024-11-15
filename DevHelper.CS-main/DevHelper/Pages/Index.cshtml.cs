using DevHelper.Data.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevHelper.Razor.Shared.Index
{
    public class CreateModel : PageModel
    {
        private readonly Login _loginService;

        public CreateModel(Login loginService)
        {
            _loginService = loginService;
            Login = new Login(); // Instanciar Login aqui
        }

        [BindProperty]
        public Login Login { get; set; }


        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        { // Verifica se o usuário já está autenticado
          if (User.Identity.IsAuthenticated) 
            { 
                return RedirectToPage("/PgProblema/Index"); 
            } 
            return Page(); 
        }
            public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _loginService.LoginAsync(Login.Email, Login.Senha);
            if (user == null)
            {
                ErrorMessage = "Registro inválido.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true // Tornar o login persistente, se necessário
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToPage("/PgProblema/Index");
        }
    }
}







//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using DevHelper.Data.Model;

//namespace DevHelper.Razor.Pages
//{
//    public class IndexModel : PageModel
//    {
//        private readonly ILogger<IndexModel> _logger;

//        public IndexModel(ILogger<IndexModel> logger)
//        {
//            _logger = logger;
//        }

//        public void OnGet()
//        {

//        }
//    }
//}
