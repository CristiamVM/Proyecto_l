using ReservasHoteles.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Interfaces
{
    public interface IRepositorioHabitaciones
    {
        List<Habitacion> ObtenerTodas();
        void Agregar(Habitacion habitacion);
        Habitacion ObtenerPorNumero(int numero);
    }
}
