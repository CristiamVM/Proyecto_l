using ReservasHoteles.Entidades;
using ReservasHoteles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Repositorios
{
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private readonly List<Usuario> usuarios = new();

        public Usuario ObtenerPorUsuarioId(string usuarioId)
            => usuarios.FirstOrDefault(u => u.UsuarioId == usuarioId);

        public Usuario ObtenerPorId(int id)
            => usuarios.FirstOrDefault(u => u.Id == id);

        public void Agregar(Usuario usuario)
            => usuarios.Add(usuario);

        public List<Usuario> ObtenerTodos() => usuarios;
    }
}
