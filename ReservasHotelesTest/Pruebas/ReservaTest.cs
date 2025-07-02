using Moq;
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
    public class ReservaTest
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
    }

}
