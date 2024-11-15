using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevHelper.Data.Model
{
    public class Login
    {
        private readonly DBdevhelperContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public Login()
        {
        }

        public Login(DBdevhelperContext context, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        public async Task<Usuario> LoginAsync(string email, string password)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Senha, password);
            if (verificationResult == PasswordVerificationResult.Failed)
                return null;

            return user;
        }

        public void Register(Usuario user, string password)
        {
            user.Senha = _passwordHasher.HashPassword(user, password);
            _context.Usuarios.Add(user);
            _context.SaveChanges();
        }
    }
}




