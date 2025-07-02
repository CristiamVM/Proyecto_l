using ReservasHoteles.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Interfaces
{
public interface IRepositorioUsuarios
{
    Usuario ObtenerPorUsuarioId(string usuarioId);
    Usuario ObtenerPorId(int id);
    void Agregar(Usuario usuario);
    List<Usuario> ObtenerTodos();
}

}
