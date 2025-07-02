using ReservasHoteles.Entidades;
using ReservasHoteles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Logica
{
    public class ClienteHotel
    {
     
        private readonly IRepositorioUsuarios repositorioUsuarios;
        private readonly IRepositorioHabitaciones repositorioHabitaciones;
        private readonly IRepositorioReservas repositorioReservas;

        private int siguienteReservaId;

        public ClienteHotel(
            IRepositorioUsuarios usuarios,
            IRepositorioHabitaciones habitaciones,
            IRepositorioReservas reservas,
            int siguienteReservaIdInicial = 1)
        {
            repositorioUsuarios = usuarios;
            repositorioHabitaciones = habitaciones;
            repositorioReservas = reservas;
            siguienteReservaId = siguienteReservaIdInicial;
        }

        public List<Habitacion> BuscarHabitaciones(string tipo = null, decimal? precioMax = null, bool soloDisponibles = false)
        {
            return repositorioHabitaciones.ObtenerTodas().Where(h =>
                (tipo == null || h.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase)) &&
                (!precioMax.HasValue || h.PrecioPorNoche <= precioMax.Value) &&
                (!soloDisponibles || h.Disponible)
            ).ToList();
        }

        public Reserva ReservarHabitacion(int numeroHabitacion, string usuarioId, DateTime fechaInicio, DateTime fechaFin)
        {
            var habitacion = repositorioHabitaciones.ObtenerPorNumero(numeroHabitacion);
            var cliente = repositorioUsuarios.ObtenerPorUsuarioId(usuarioId);

            if (habitacion == null || cliente == null || !habitacion.Disponible || cliente.Rol != "cliente")
            {
                Console.WriteLine("Habitación no disponible, cliente inválido o no autorizado.");
                return null;
            }

            var reserva = new Reserva
            {
                Id = siguienteReservaId++,
                Cliente = cliente,
                Habitacion = habitacion,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            repositorioReservas.Agregar(reserva);
            habitacion.Disponible = false;

            Console.WriteLine("Reserva realizada correctamente.");
            return reserva;
        }

        public bool CancelarReserva(int idReserva, string usuarioId)
        {
            var reserva = repositorioReservas.ObtenerPorId(idReserva);
            if (reserva != null && !reserva.Cancelada && reserva.Cliente.UsuarioId == usuarioId)
            {
                reserva.Cancelada = true;
                reserva.Habitacion.Disponible = true;
                Console.WriteLine("Reserva cancelada correctamente.");
                return true;
            }

            Console.WriteLine("Reserva no encontrada, ya cancelada o no pertenece al usuario.");
            return false;
        }

        public List<Reserva> ObtenerHistorialReservas(string usuarioId)
        {
            return repositorioReservas.ObtenerTodas()
                .Where(r => r.Cliente.UsuarioId == usuarioId)
                .ToList();
        }

        public List<Habitacion> VerDisponibilidad(DateTime fechaInicio, DateTime fechaFin)
        {
            var ocupadas = repositorioReservas.ObtenerTodas()
                .Where(r => !r.Cancelada &&
                            r.FechaInicio < fechaFin &&
                            r.FechaFin > fechaInicio)
                .Select(r => r.Habitacion.Numero)
                .ToHashSet();

            return repositorioHabitaciones.ObtenerTodas()
                .Where(h => !ocupadas.Contains(h.Numero))
                .ToList();
        }
    }
}