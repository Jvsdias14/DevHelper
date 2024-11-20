using DevHelper.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DevHelper.Data.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DevHelper.Data.Repositories
{
    public class ProblemaRepository : iProblemaRepositoryAsync, iProblemaRepository
    {
        private DBdevhelperContext db;
        public ProblemaRepository(DBdevhelperContext context)
        {
            db = context;
        }

        public void Alterar(Problema oProblema)
        {
            db.Entry(oProblema).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public async Task AlterarAsync(Problema oProblema)
        {
            db.Entry(oProblema).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Excluir(Problema oProblema)
        {
            db.Entry(oProblema).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
        }

        public async Task ExcluirAsync(Problema oProblema)
        {
            db.Entry(oProblema).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await db.SaveChangesAsync();
        }

        public void Incluir(Problema oProblema)
        {
            db.Problemas.Add(oProblema);
            db.SaveChanges();
        }

        public async Task IncluirAsync(Problema oProblema)
        {
            db.Problemas.Add(oProblema);
            await db.SaveChangesAsync();
        }

        public Problema SelecionaPelaChave(int id)
        {
            return db.Problemas.Find(id);
        }

        public async Task<Problema> SelecionaPelaChaveAsync(int id)
        {
            return await db.Problemas.FindAsync(id);
        }

        public List<Problema> SelecionarTodos()
        {
            return db.Problemas.OrderBy(p => p.Id).ToList();
        }

        public async Task<List<Problema>> SelecionTodosAsync()
        {
            return await db.Problemas.OrderBy(p => p.Nome).ToListAsync();
        }
        public async Task<List<Problema>> Pesquisar(string query)
        {
             
            return await db.Problemas
                .Where(p => p.Nome.Contains(query))
                .ToListAsync();
        }

        public async Task<List<Problema>> SelecionarProblemaUsuario()
        {
            var problemas = await SelecionTodosAsync();
            foreach (var problema in problemas)
            {
                problema.Usuario = await db.Usuarios.FindAsync(problema.UsuarioId);
            }
            return problemas;
        }
    }
 }
