using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Entidades
{
    public static class Reporte
    {
        public static void GenerarReporteOcupacion(List<Reserva> reservas, DateTime desde, DateTime hasta)
        {
            var total = reservas.Count;
            var activas = reservas.Count(r =>
                !r.Cancelada &&
                r.FechaInicio < hasta &&
                r.FechaFin > desde
            );

            Console.WriteLine(" Reporte de Ocupación");
            Console.WriteLine($"Total de reservas: {total}");
            Console.WriteLine($"Reservas activas entre {desde:d} y {hasta:d}: {activas}");
        }
    }
}
