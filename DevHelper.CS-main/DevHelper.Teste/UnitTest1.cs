using DevHelper.Data;
using DevHelper.Data.Repositories;
using DevHelper.Data.Model;
using DevHelper.Data.Repository;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;

namespace DevHelper.Teste
{
    public class TesteCadUsuario
    {
        private DBdevhelperContext db;
        private UsuarioRepository _Repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DBdevhelperContext>()
                .UseSqlServer("Server=.\\SQLEXPRESS;Database=devhelper;Trusted_connection=True;TrustServerCertificate=True")
                .Options;

            db = new DBdevhelperContext(options);
            _Repository = new UsuarioRepository(db);
        }

        [Test]
        public void Incluir()
        {
            Usuario oUsuario = new Usuario
            {
                Nome = "UserTESTE",
                Email = "teste@teste",
                Senha = "123456"
            };
            _Repository.Incluir(oUsuario);
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            db.Dispose();
        }
    }
}
