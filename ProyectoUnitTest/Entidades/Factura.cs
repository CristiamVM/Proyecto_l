using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Entidades
{
    public class Factura
    {
        public int Id { get; set; }
        public Usuario Cliente { get; set; }
        public Reserva Reserva { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Total { get; set; }

        public override string ToString()
        {
            return $"Factura #{Id}\nCliente: {Cliente.Nombre}\nReserva #{Reserva.Id}\nTotal: {Total:C}\nFecha: {FechaEmision:d}";
        }
    }

}
