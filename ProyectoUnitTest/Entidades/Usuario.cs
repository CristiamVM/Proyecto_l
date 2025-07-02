using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }                    // ID único
        public string UsuarioId { get; set; }          // Nombre de usuario para login
        public string Contraseña { get; set; }         // Contraseña de acceso
        public string Rol { get; set; }                // "admin" o "cliente"

        // Datos de cliente (solo aplican si es cliente)
        public string Nombre { get; set; }
        public string Correo { get; set; }

        public override string ToString()
        {
            return $"[{Rol.ToUpper()}] {Nombre ?? UsuarioId} ({UsuarioId})";
        }

    }
}