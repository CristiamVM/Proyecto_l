using ReservasHoteles.Entidades;
using ReservasHoteles.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Logica
{

    public class AdministradorHotel
    {
        private readonly HotelAdmin hotel;
        private int siguienteUsuarioId = 1;  // Asignador incremental

        public AdministradorHotel(HotelAdmin hotelAdmin)
        {
            hotel = hotelAdmin;
        }

        private bool ValidarAdmin(string usuarioId)
        {
            var usuario = hotel.RepositorioUsuarios.ObtenerPorUsuarioId(usuarioId);
            if (usuario == null || usuario.Rol != "admin")
            {
                Console.WriteLine(" Acceso denegado. Solo administradores pueden realizar esta acción.");
                return false;
            }
            return true;
        }


        public Usuario RegistrarCliente(string adminUsuarioId, string usuarioId, string contraseña, string nombre, string correo)
        {
            if (!ValidarAdmin(adminUsuarioId)) return null;

            if (hotel.RepositorioUsuarios.ObtenerPorUsuarioId(usuarioId) != null)
            {
                Console.WriteLine("El usuario ya existe.");
                return null;
            }

            var cliente = new Usuario
            {
                Id = siguienteUsuarioId++,
                UsuarioId = usuarioId,
                Contraseña = contraseña,
                Nombre = nombre,
                Correo = correo,
                Rol = "cliente"
            };

            hotel.RepositorioUsuarios.Agregar(cliente);
            Console.WriteLine("Cliente registrado correctamente.");
            return cliente;
        }


        public void RegistrarHabitacion(string adminUsuarioId, int numero, string tipo, decimal precio)
        {
            if (!ValidarAdmin(adminUsuarioId)) return;

            var existente = hotel.RepositorioHabitaciones.ObtenerPorNumero(numero);
            if (existente != null)
            {
                Console.WriteLine("Ya existe una habitación con ese número.");
                return;
            }

            hotel.RepositorioHabitaciones.Agregar(new Habitacion
            {
                Numero = numero,
                Tipo = tipo,
                PrecioPorNoche = precio
            });

            Console.WriteLine("Habitación registrada correctamente.");
        }


        public void GenerarFactura(int idReserva, string adminUsuarioId)
        {
            if (!ValidarAdmin(adminUsuarioId)) return;

            var reserva = hotel.RepositorioReservas.ObtenerPorId(idReserva);
            if (reserva == null || reserva.Cancelada)
            {
                Console.WriteLine("Reserva no válida para facturación.");
                return;
            }

            int noches = (int)(reserva.FechaFin - reserva.FechaInicio).TotalDays;
            decimal total = noches * reserva.Habitacion.PrecioPorNoche;

            Console.WriteLine($"\n--- FACTURA #{idReserva} ---");
            Console.WriteLine($"Cliente: {reserva.Cliente.Nombre} ({reserva.Cliente.UsuarioId})");
            Console.WriteLine($"Habitación: {reserva.Habitacion.Numero}");
            Console.WriteLine($"Estadía: {reserva.FechaInicio:dd/MM/yyyy} - {reserva.FechaFin:dd/MM/yyyy}");
            Console.WriteLine($"Precio por noche: {reserva.Habitacion.PrecioPorNoche:C}");
            Console.WriteLine($"Total a pagar: {total:C}");
            Console.WriteLine("---------------------------");

            reserva.Habitacion.Disponible = true;
        }


        public void GenerarReporteOcupacion(DateTime desde, DateTime hasta, string adminUsuarioId)
        {
            if (!ValidarAdmin(adminUsuarioId)) return;

            var reservas = hotel.RepositorioReservas.ObtenerTodas()
                .Where(r => !r.Cancelada &&
                            r.FechaInicio < hasta &&
                            r.FechaFin > desde)
                .ToList();

            var habitaciones = hotel.RepositorioHabitaciones.ObtenerTodas();
            double ocupadas = reservas.Select(r => r.Habitacion.Numero).Distinct().Count();
            double total = habitaciones.Count;

            Console.WriteLine("\nReporte de Ocupación");
            Console.WriteLine($"Rango: {desde:dd/MM/yyyy} a {hasta:dd/MM/yyyy}");
            Console.WriteLine($"Habitaciones ocupadas: {ocupadas} / {total}");
            Console.WriteLine($"Tasa de ocupación: {ocupadas / total:P2}");
        }


        public void EnviarRecordatoriosCheckIn(string adminUsuarioId)
        {
            if (!ValidarAdmin(adminUsuarioId)) return;

            DateTime mañana = DateTime.Today.AddDays(1);

            var reservas = hotel.RepositorioReservas.ObtenerTodas()
                .Where(r => !r.Cancelada && r.FechaInicio.Date == mañana.Date)
                .ToList();

            Console.WriteLine("\n Recordatorios de Check-In para mañana:");
            foreach (var r in reservas)
            {
                Console.WriteLine($"- A {r.Cliente.Nombre} ({r.Cliente.Correo}) para habitación #{r.Habitacion.Numero} - {r.FechaInicio:dd/MM/yyyy}");
            }

            if (reservas.Count == 0)
                Console.WriteLine("No hay check-ins programados para mañana.");
        }
    }


}
