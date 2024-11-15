using DevHelper.Data.Interface;
using DevHelper.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Repository
{
    public class ArquivoProblemaRepository : iArquivoProblemaRepository, iArquivoProblemaRepositoryAsync
    {
        private DBdevhelperContext db;
        public ArquivoProblemaRepository(DBdevhelperContext context)
        {
            db = context;
        }

        public void Alterar(ArquivoProblema arquivoproblema)
        {
            db.Entry(arquivoproblema).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public async Task AlterarAsync(ArquivoProblema arquivoproblema)
        {
            db.Entry(arquivoproblema).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Excluir(ArquivoProblema arquivoproblema)
        {
            db.Entry(arquivoproblema).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
        }

        public async Task ExcluirAsync(ArquivoProblema arquivoproblema)
        {
            db.Entry(arquivoproblema).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await db.SaveChangesAsync();
        }

        public void Incluir(ArquivoProblema arquivoproblema)
        {
            db.ArquivoProblemas.Add(arquivoproblema);
            db.SaveChanges();
        }

        public async Task IncluirAsync(ArquivoProblema arquivoproblema)
        {
            db.ArquivoProblemas.Add(arquivoproblema);
            await db.SaveChangesAsync();
        }

        public ArquivoProblema SelecionaPelaChave(int id)
        {
            return db.ArquivoProblemas.Find(id);
        }

        public async Task<ArquivoProblema> SelecionaPelaChaveAsync(int id)
        {
            return await db.ArquivoProblemas.FindAsync(id);
        }

        public List<ArquivoProblema> SelecionarTodos()
        {
            return db.ArquivoProblemas.OrderBy(p => p.Id).ToList();
        }

        public async Task<List<ArquivoProblema>> SelecionTodosAsync()
        {
            return await db.ArquivoProblemas.OrderBy(p => p.Nome).ToListAsync();
        }
    }
}
