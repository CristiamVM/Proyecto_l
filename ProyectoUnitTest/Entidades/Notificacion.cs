using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Entidades
{
    public static class Notificacion
    {
        public static void EnviarRecordatorioCheckIn(Reserva reserva)
        {
            Console.WriteLine($" Notificación enviada a {reserva.Cliente.Correo} " +
                $"para el check-in el {reserva.FechaInicio:dd/MM/yyyy} de la habitación #{reserva.Habitacion.Numero}.");
        }
    }

}
