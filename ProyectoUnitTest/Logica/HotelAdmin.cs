using ReservasHoteles.Entidades;
using ReservasHoteles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Logica
{
    //Clase admin
    public class HotelAdmin
    {
        public IRepositorioHabitaciones RepositorioHabitaciones { get; }
        public IRepositorioUsuarios RepositorioUsuarios { get; }
        public IRepositorioReservas RepositorioReservas { get; }

        public int SiguienteUsuarioId { get; set; } = 1;
        public int SiguienteReservaId { get; set; } = 1;

        public HotelAdmin(IRepositorioHabitaciones rh, IRepositorioUsuarios ru, IRepositorioReservas rr)
        {
            RepositorioHabitaciones = rh;
            RepositorioUsuarios = ru;
            RepositorioReservas = rr;
        }
    }
}
