using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Entidades
{
    public class Habitacion
    {
        public int? Numero { get; set; }
        public string Tipo { get; set; } // Ej: "Simple", "Doble", "Suite"
        public decimal PrecioPorNoche { get; set; }
        public bool Disponible { get; set; } = true;
    }

}
