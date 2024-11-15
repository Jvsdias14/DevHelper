using DevHelper.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevHelper.Data.Interface
{
    public interface iUsuarioRepositoryAsync
    {
        Task IncluirAsync(Usuario usuario);
        Task AlterarAsync(Usuario usuario);
        Task ExcluirAsync(Usuario usuario);
        Task<Usuario> SelecionaPelaChaveAsync(int id);
        Task<List<Usuario>> SelecionTodosAsync();
    }
}
