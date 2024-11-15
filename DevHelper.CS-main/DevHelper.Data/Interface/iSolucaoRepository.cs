using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iSolucaoRepository
    {
        void Incluir(Solucao solucao);
        void Alterar(Solucao solucao);
        void Excluir(Solucao solucao);
        Solucao SelecionaPelaChave(int id);
        List<Solucao> SelecionarTodos();
    }
}
