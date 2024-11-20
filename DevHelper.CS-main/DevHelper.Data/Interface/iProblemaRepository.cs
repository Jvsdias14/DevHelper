using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interfaces
{
    public interface iProblemaRepository
    {
        void Incluir(Problema oProblema);
        void Alterar(Problema oProblema);
        void Excluir(Problema oProblema);
        Problema SelecionaPelaChave(int id);
        List<Problema> SelecionarTodos();
        
    }
}
