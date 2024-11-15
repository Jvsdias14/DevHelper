using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iArquivoProblemaRepositoryAsync
    {
        Task IncluirAsync(ArquivoProblema arquivoproblema);
        Task AlterarAsync(ArquivoProblema arquivoproblema);
        Task ExcluirAsync(ArquivoProblema arquivoproblema);
        Task<ArquivoProblema> SelecionaPelaChaveAsync(int id);
        Task<List<ArquivoProblema>> SelecionTodosAsync();
    }
}
