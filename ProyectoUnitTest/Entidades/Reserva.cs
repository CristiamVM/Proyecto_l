using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Entidades
{
    public class Reserva
    {
        public int Id { get; set; }
        public Usuario Cliente { get; set; }
        public Habitacion Habitacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Cancelada { get; set; } = false;
    }

}
