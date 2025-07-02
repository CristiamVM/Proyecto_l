using ReservasHoteles.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Interfaces
{
    public interface IRepositorioReservas
    {
        List<Reserva> ObtenerTodas();
        void Agregar(Reserva reserva);
        Reserva ObtenerPorId(int id);
    }
}
