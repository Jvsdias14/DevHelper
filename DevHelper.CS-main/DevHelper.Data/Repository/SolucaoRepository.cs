﻿using DevHelper.Data.Interface;
using DevHelper.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevHelper.Data.Repository
{
    public class SolucaoRepository : iSolucaoRepository, iSolucaoRepositoryAsync
    {
        private readonly DBdevhelperContext db;

        public SolucaoRepository(DBdevhelperContext context)
        {
            db = context;
        }

        public void Alterar(Solucao solucao)
        {
            db.Entry(solucao).State = EntityState.Modified;
            db.SaveChanges();
        }

        public async Task AlterarAsync(Solucao solucao)
        {
            db.Entry(solucao).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void Excluir(Solucao solucao)
        {
            db.Entry(solucao).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public async Task ExcluirAsync(Solucao solucao)
        {
            db.Entry(solucao).State = EntityState.Deleted;
            await db.SaveChangesAsync();
        }

        public void Incluir(Solucao solucao)
        {
            db.Solucoes.Add(solucao);
            db.SaveChanges();
        }

        public async Task IncluirAsync(Solucao solucao)
        {
            db.Solucoes.Add(solucao);
            await db.SaveChangesAsync();
        }

        public Solucao SelecionaPelaChave(int id)
        {
            return db.Solucoes.Find(id);
        }

        public async Task<Solucao> SelecionaPelaChaveAsync(int id)
        {
            return await db.Solucoes.FindAsync(id);
        }

        public List<Solucao> SelecionarTodos()
        {
            return db.Solucoes.OrderBy(p => p.Id).ToList();
        }

        public async Task<List<Solucao>> SelecionTodosAsync()
        {
            return await db.Solucoes.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<List<Solucao>> SeleccionarSolucoesPorProblema(int idproblema)
        {
            return await db.Solucoes.Where(p => p.ProblemaId == idproblema).ToListAsync();
        }
    }
}
