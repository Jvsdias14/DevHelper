using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iArquivoSolucaoRepositoryAsync
    {
        Task IncluirAsync(ArquivoSolucao arquivosolucao);
        Task AlterarAsync(ArquivoSolucao arquivosolucao);
        Task ExcluirAsync(ArquivoSolucao arquivosolucao);
        Task<ArquivoSolucao> SelecionaPelaChaveAsync(int id);
        Task<List<ArquivoSolucao>> SelecionTodosAsync();
    }
}
