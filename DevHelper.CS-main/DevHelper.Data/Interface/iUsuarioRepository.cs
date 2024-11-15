using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iUsuarioRepository
    {
        void Incluir(Usuario usuario);
        void Alterar(Usuario usuario);
        void Excluir(Usuario usuario);
        Usuario SelecionaPelaChave(int id);
        List<Usuario> SelecionarTodos();
    }

}
