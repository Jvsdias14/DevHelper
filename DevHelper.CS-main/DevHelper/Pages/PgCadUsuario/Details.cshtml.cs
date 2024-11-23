using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using DevHelper.Data.Model;
using System.Security.Claims;
using DevHelper.Data.Interface;
using DevHelper.Data.Repository;
using DevHelper.Data.Interfaces;

namespace DevHelper.Razor.Pages.PgCadUsuario
{
    public class DetailsModel : PageModel
    {
        private readonly iUsuarioRepositoryAsync UsuarioRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly iProblemaRepositoryAsync ProblemaRepository;
        private readonly iSolucaoRepositoryAsync SolucaoRepository;

        public DetailsModel(iUsuarioRepositoryAsync usuariorepositoryasync, IHttpContextAccessor httpContextAccessor, iSolucaoRepositoryAsync solucaoRepository, iProblemaRepositoryAsync problemarepository)
        {
            UsuarioRepository = usuariorepositoryasync;
            _httpContextAccessor = httpContextAccessor;
            SolucaoRepository = solucaoRepository;
            ProblemaRepository = problemarepository;
        }

        [BindProperty]
        public Usuario Usuario { get; set; } = default!;

        public bool IsCurrentUser { get; set; }
        public ICollection<Problema> Problemas { get; set; } = default!;
        public ICollection<Solucao> Solucoes { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await UsuarioRepository.ObterUsuarioComProblemasESolucoesAsync(id.Value);
            if (usuario == null)
            {
                return NotFound();
            }

            Usuario = usuario;
            Problemas = usuario.Problemas ?? new List<Problema>();
            Solucoes = usuario.Solucaos ?? new List<Solucao>();

            foreach (var solucao in Solucoes)
            {
                solucao.Problema = await ProblemaRepository.SelecionaPelaChaveAsync(solucao.ProblemaId);
            }

            var loggedInUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IsCurrentUser = loggedInUserId == Usuario.Id.ToString();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Usuario == null)
            {
                return NotFound();
            }

            var loggedInUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            // Buscar o usuário no banco de dados com os valores antigos
            var usuarioToUpdate = await UsuarioRepository.SelecionaPelaChaveAsync(loggedInUserId);

            if (usuarioToUpdate == null)
            {
                return NotFound();
            }

            // Atualizar a biografia com o novo valor vindo do front-end
            usuarioToUpdate.Biografia = Usuario.Biografia;

            // Salvar as alterações no banco de dados
            await UsuarioRepository.AlterarAsync(usuarioToUpdate);



            //return RedirectToPage("/PgCadUsuario/Details", new { id = loggedInUserId });

            return RedirectToPage("../Index");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("../Index");
        }
    }
}
