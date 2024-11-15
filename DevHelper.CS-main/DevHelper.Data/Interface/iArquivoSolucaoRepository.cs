using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iArquivoSolucaoRepository
    {
        void Incluir(ArquivoSolucao arquivosolucao);
        void Alterar(ArquivoSolucao arquivosolucao);
        void Excluir(ArquivoSolucao arquivosolucao);
        ArquivoSolucao SelecionaPelaChave(int id);
        List<ArquivoSolucao> SelecionarTodos();
    }
}
