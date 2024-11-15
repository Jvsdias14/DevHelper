using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevHelper.Data.Model;
using DevHelper.Data.Interface;
using Microsoft.EntityFrameworkCore;

namespace DevHelper.Data.Repository
{
    public class UsuarioRepository : iUsuarioRepository, iUsuarioRepositoryAsync
    {
        private DBdevhelperContext db;
        public UsuarioRepository(DBdevhelperContext context)
        {
            db = context;
        }
        public void Alterar(Usuario usuario)
        {
            db.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public async Task AlterarAsync(Usuario usuario)
        {
            db.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Excluir(Usuario usuario)
        {
            db.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
        }

        public async Task ExcluirAsync(Usuario usuario)
        {
            db.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await db.SaveChangesAsync();
        }

        public void Incluir(Usuario usuario)
        {
            db.Usuarios.Add(usuario);
            db.SaveChanges();
        }

        public async Task IncluirAsync(Usuario usuario)
        {
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
        }

        public Usuario SelecionaPelaChave(int id)
        {
            return db.Usuarios.Find(id);
        }

        public async Task<Usuario> SelecionaPelaChaveAsync(int id)
        {
            return await db.Usuarios.FindAsync(id);
        }

        public List<Usuario> SelecionarTodos()
        {
            return db.Usuarios.OrderBy(p => p.Id).ToList();
        }

        public async Task<List<Usuario>> SelecionTodosAsync()
        {
            return await db.Usuarios.OrderBy(p => p.Nome).ToListAsync();
        }
    }
}
