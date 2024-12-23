﻿using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interfaces
{
    public interface iProblemaRepositoryAsync
    {
        Task IncluirAsync(Problema oProblema);
        Task AlterarAsync(Problema oProblema);
        Task ExcluirAsync(Problema oProblema);
        Task<Problema> SelecionaPelaChaveAsync(int id);
        Task<List<Problema>> SelecionTodosAsync();

        Task <List<Problema>> SelecionarProblemaComTudo();
        Task <List<Problema>> Pesquisar(string query);
        Task<List<Solucao>> BuscarSolucoesRestantesAsync(int problemaId, int numeroDeSolucoesCarregadas);

        Task<Problema> ProblemaCompletoAsync(int problemaId);
    }
}
