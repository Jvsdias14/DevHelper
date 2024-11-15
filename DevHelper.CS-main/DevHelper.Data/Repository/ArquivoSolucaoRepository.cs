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
    public class ArquivoSolucaoRepository : iArquivoSolucaoRepository, iArquivoSolucaoRepositoryAsync
    {
        private DBdevhelperContext db;
        public ArquivoSolucaoRepository(DBdevhelperContext context)
        {
            db = context;
        }

        public void Alterar(ArquivoSolucao arquivosolucao)
        {
            db.Entry(arquivosolucao).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public async Task AlterarAsync(ArquivoSolucao arquivosolucao)
        {
            db.Entry(arquivosolucao).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Excluir(ArquivoSolucao arquivosolucao)
        {
            db.Entry(arquivosolucao).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
        }

        public async Task ExcluirAsync(ArquivoSolucao arquivosolucao)
        {
            db.Entry(arquivosolucao).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await db.SaveChangesAsync();
        }

        public void Incluir(ArquivoSolucao arquivosolucao)
        {
            db.ArquivoSolucoes.Add(arquivosolucao);
            db.SaveChanges();
        }

        public async Task IncluirAsync(ArquivoSolucao arquivosolucao)
        {
            db.ArquivoSolucoes.Add(arquivosolucao);
            await db.SaveChangesAsync();
        }

        public ArquivoSolucao SelecionaPelaChave(int id)
        {
            return db.ArquivoSolucoes.Find(id);
        }

        public async Task<ArquivoSolucao> SelecionaPelaChaveAsync(int id)
        {
            return await db.ArquivoSolucoes.FindAsync(id);
        }

        public List<ArquivoSolucao> SelecionarTodos()
        {
            return db.ArquivoSolucoes.OrderBy(p => p.Id).ToList();
        }

        public async Task<List<ArquivoSolucao>> SelecionTodosAsync()
        {
            return await db.ArquivoSolucoes.OrderBy(p => p.Nome).ToListAsync();
        }
    }
}
