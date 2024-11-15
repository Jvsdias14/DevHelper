using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iArquivoProblemaRepository
    {
        void Incluir(ArquivoProblema arquivoproblema);
        void Alterar(ArquivoProblema arquivoproblema);
        void Excluir(ArquivoProblema arquivoproblema);
        ArquivoProblema SelecionaPelaChave(int id);
        List<ArquivoProblema> SelecionarTodos();
    }
}
