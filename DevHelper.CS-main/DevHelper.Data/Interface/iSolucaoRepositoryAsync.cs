using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iSolucaoRepositoryAsync
    {
        Task IncluirAsync(Solucao solucao);
        Task AlterarAsync(Solucao solucao);
        Task ExcluirAsync(Solucao solucao);
        Task<Solucao> SelecionaPelaChaveAsync(int id);
        Task<List<Solucao>> SelecionTodosAsync();
    }
}
