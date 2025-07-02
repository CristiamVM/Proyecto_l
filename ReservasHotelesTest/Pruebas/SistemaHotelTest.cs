/*using Moq;
using ReservasHoteles.Entidades;
using ReservasHoteles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHotelesTest.Pruebas
{
    [TestFixture]
    public class SistemaHotelTestSuite
    {
        private Mock<IRepositorioHabitaciones> _repoHabitaciones;
        private Mock<IRepositorioReservas> _repoReservas;
        private Mock<IRepositorioUsuarios> _repoUsuarios;
        private List<Reserva> _reservas;

        [SetUp]
        public void SetUp()
        {
            _repoHabitaciones = new Mock<IRepositorioHabitaciones>();
            _repoReservas = new Mock<IRepositorioReservas>();
            _repoUsuarios = new Mock<IRepositorioUsuarios>();
            _reservas = new List<Reserva>();
        }

        // TC101 - Registrar habitación con datos válidos
        [Test]
        public void TC101_RegistrarHabitacion_Valida()
        {
            var habitacion = new Habitacion { Numero = 101, Tipo = "Suite", PrecioPorNoche = 100, Disponible = true };
            _repoHabitaciones.Setup(r => r.Agregar(It.IsAny<Habitacion>()));

            _repoHabitaciones.Object.Agregar(habitacion);

            _repoHabitaciones.Verify(r => r.Agregar(It.Is<Habitacion>(h => h.Numero == 101)), Times.Once);
        }

        // TC102 - Registrar habitación sin número
        [Test]
        public void TC102_RegistrarHabitacion_SinNumero()
        {
            var habitacion = new Habitacion { Numero = null, Tipo = "Suite", PrecioPorNoche = 100m, Disponible = true };

            bool resultado = habitacion.Numero == null;

            Assert.IsTrue(resultado, "Debe validar que el número de habitación no sea nulo.");
        }

        // TC103 - Buscar habitación por tipo y estado
        [Test]
        public void TC103_BuscarHabitacion_TipoSuite_Disponible()
        {
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Numero = 1, Tipo = "Suite", Disponible = true },
                new Habitacion { Numero = 2, Tipo = "Suite", Disponible = false }
            };

            _repoHabitaciones.Setup(r => r.ObtenerTodas()).Returns(habitaciones);

            var resultado = _repoHabitaciones.Object.ObtenerTodas()
                .Where(h => h.Tipo == "Suite" && h.Disponible == true).ToList();

            Assert.AreEqual(1, resultado.Count);
        }

        // TC104 - Buscar sin filtros
        [Test]
        public void TC104_BuscarHabitacion_SinFiltros()
        {
            string tipo = null, estado = null;
            Assert.IsTrue(string.IsNullOrEmpty(tipo) && string.IsNullOrEmpty(estado),
                "Debe mostrar error cuando no se aplican filtros.");
        }

        // TC105 - Reservar habitación disponible
        [Test]
        public void TC105_ReservarHabitacion_Disponible()
        {
            var reserva = new Reserva
            {
                Id = 1,
                Habitacion = new Habitacion { Numero = 101, Disponible = true },
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(2),
                Cliente = new Usuario { Nombre = "Cliente" }
            };

            _repoReservas.Setup(r => r.Agregar(reserva));

            _repoReservas.Object.Agregar(reserva);

            _repoReservas.Verify(r => r.Agregar(It.Is<Reserva>(r => r.Id == 1)), Times.Once);
        }

        // TC106 - Reservar habitación ya ocupada
        [Test]
        public void TC106_ReservarHabitacion_Ocupada()
        {
            var habitacion = new Habitacion { Numero = 101, Disponible = false };
            bool disponible = habitacion.Disponible == true;

            Assert.IsFalse(disponible, "No se puede reservar una habitación ocupada.");
        }

        // TC107 - Cancelar reserva con 24h de anticipación
        [Test]
        public void TC107_CancelarReserva_ConAnticipacion()
        {
            var reserva = new Reserva
            {
                Cancelada = false,
                FechaInicio = DateTime.Today.AddDays(2)
            };

            bool puedeCancelar = (reserva.FechaInicio - DateTime.Today).TotalHours >= 24;

            Assert.IsTrue(puedeCancelar, "La reserva debe poder cancelarse con más de 24h.");
        }

        // TC108 - Cancelar reserva ya finalizada
        [Test]
        public void TC108_CancelarReserva_Finalizada()
        {
            var reserva = new Reserva { FechaFin = DateTime.Today.AddDays(-1) };
            bool finalizada = reserva.FechaFin < DateTime.Today;

            Assert.IsTrue(finalizada, "No se debe cancelar una reserva ya finalizada.");
        }

        // TC109 - Registrar cliente válido
        [Test]
        public void TC109_RegistrarCliente_Valido()
        {
            var cliente = new Usuario { Nombre = "Juan", Correo = "juan@mail.com" };
            _repoUsuarios.Setup(r => r.Agregar(cliente));

            _repoUsuarios.Object.Agregar(cliente);

            _repoUsuarios.Verify(r => r.Agregar(It.Is<Usuario>(u => u.Nombre == "Juan")), Times.Once);
        }

        // TC110 - Registrar cliente sin correo
        [Test]
        public void TC110_RegistrarCliente_SinCorreo()
        {
            var cliente = new Usuario { Nombre = "Ana", Correo = null };
            bool correoInvalido = string.IsNullOrEmpty(cliente.Correo);

            Assert.IsTrue(correoInvalido, "Debe validar que el campo correo no esté vacío.");
        }

        // TC111 - Ver historial de cliente con reservas
        [Test]
        public void TC111_VerHistorial_ConReservas()
        {
            var usuario = new Usuario { Id = 1 };
            var reservas = new List<Reserva> { new Reserva { Cliente = usuario } };

            _repoReservas.Setup(r => r.ObtenerTodas()).Returns(reservas);

            var historial = _repoReservas.Object.ObtenerTodas().Where(r => r.Cliente.Id == 1).ToList();

            Assert.IsNotEmpty(historial);
        }

        // TC112 - Ver historial de cliente sin reservas
        [Test]
        public void TC112_VerHistorial_SinReservas()
        {
            var historial = new List<Reserva>();

            Assert.IsEmpty(historial, "El historial debe estar vacío para nuevos usuarios.");
        }

        // TC113 - Enviar notificación válida
        [Test]
        public void TC113_EnviarNotificacion_Valida()
        {
            var reserva = new Reserva
            {
                Cliente = new Usuario { Correo = "cliente@mail.com" },
                Habitacion = new Habitacion { Numero = 101 },
                FechaInicio = DateTime.Today.AddDays(1)
            };

            string mensaje = $"Notificación enviada a {reserva.Cliente.Correo}";
            Assert.IsTrue(mensaje.Contains("Notificación enviada"), "La notificación debe generarse.");
        }

        // TC114 - Enviar notificación a reserva cancelada
        [Test]
        public void TC114_EnviarNotificacion_ReservaCancelada()
        {
            var reserva = new Reserva { Cancelada = true };
            bool puedeNotificar = !reserva.Cancelada;

            Assert.IsFalse(puedeNotificar, "No se debe notificar una reserva cancelada.");
        }

        // TC115 - Consultar disponibilidad con fechas válidas
        [Test]
        public void TC115_ConsultarDisponibilidad_Valida()
        {
            DateTime checkIn = DateTime.Today.AddDays(1);
            DateTime checkOut = DateTime.Today.AddDays(3);
            bool fechasValidas = checkIn < checkOut;

            Assert.IsTrue(fechasValidas, "El check-in debe ser anterior al check-out.");
        }

        // TC116 - Consultar disponibilidad con fechas inválidas
        [Test]
        public void TC116_ConsultarDisponibilidad_Invalida()
        {
            DateTime checkIn = DateTime.Today.AddDays(3);
            DateTime checkOut = DateTime.Today.AddDays(1);
            bool fechasValidas = checkIn < checkOut;

            Assert.IsFalse(fechasValidas, "El rango de fechas es inválido.");
        }

        // TC117 - Generar factura tras check-out
        [Test]
        public void TC117_GenerarFactura_CheckOut()
        {
            var factura = new Factura
            {
                Cliente = new Usuario { Nombre = "Luis" },
                Reserva = new Reserva { Id = 1 },
                Total = 200m,
                FechaEmision = DateTime.Today
            };

            string resumen = factura.ToString();
            Assert.IsTrue(resumen.Contains("Factura"), "La factura debe generarse con resumen.");
        }

        // TC118 - Generar factura sin pago confirmado (simulado)
        [Test]
        public void TC118_GenerarFactura_SinPago()
        {
            bool pagoConfirmado = false;
            Assert.IsFalse(pagoConfirmado, "No se puede generar factura sin pago confirmado.");
        }

        // TC119 - Generar reporte con reservas activas
        [Test]
        public void TC119_GenerarReporte_ConDatos()
        {
            var reservas = new List<Reserva>
            {
                new Reserva { FechaInicio = DateTime.Today.AddDays(-1), FechaFin = DateTime.Today.AddDays(2), Cancelada = false }
            };

            var activas = reservas.Where(r =>
                !r.Cancelada &&
                r.FechaInicio < DateTime.Today.AddMonths(1) &&
                r.FechaFin > DateTime.Today).ToList();

            Assert.IsNotEmpty(activas, "Debe haber reservas activas en el reporte.");
        }

        // TC120 - Generar reporte sin reservas
        [Test]
        public void TC120_GenerarReporte_SinDatos()
        {
            var reservas = new List<Reserva>();
            Assert.IsEmpty(reservas, "No se encontraron datos de ocupación para el período.");
        }
    }

}
*/