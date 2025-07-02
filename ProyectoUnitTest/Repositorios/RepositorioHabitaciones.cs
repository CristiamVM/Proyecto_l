using ReservasHoteles.Entidades;
using ReservasHoteles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Repositorios
{
    public class RepositorioHabitaciones : IRepositorioHabitaciones
    {
        private readonly List<Habitacion> habitaciones = new();

        public void Agregar(Habitacion habitacion) => habitaciones.Add(habitacion);

        public List<Habitacion> ObtenerTodas() => habitaciones;

        public Habitacion ObtenerPorNumero(int numero) =>
            habitaciones.FirstOrDefault(h => h.Numero == numero);
    }

}
