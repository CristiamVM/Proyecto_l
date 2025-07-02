using NUnit.Framework;
using Moq;
using ProyectoUnitTest.Models;
using ReservasHotelesTest.Mocks; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ReservasHotelesTest.Pruebas
{
    public class ReservaServiceTests
    {
        private Mock<DbEntities> _mockContext;
        private ReservaService _service;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<DbEntities>();
            _service = new ReservaService(_mockContext.Object);
        }

        [Test]
        public void TC105_ReservarHabitacion_Disponible_ClienteRegistrado()
        {
            var habitacion = new Habitacion { IdHabitacion = 1, Estado = "Disponible" };
            var cliente = new Cliente { IdCliente = 1 };
            var fechaInicio = DateTime.Today.AddDays(3);
            var fechaFin = fechaInicio.AddDays(2);

            var result = _service.ReservarHabitacion(habitacion, cliente, fechaInicio, fechaFin);

            Assert.IsTrue(result);
        }

        [Test]
        public void TC106_ReservarHabitacion_NoDisponible_MismaFecha()
        {
            var habitacion = new Habitacion { IdHabitacion = 1, Estado = "Ocupado" };
            var cliente = new Cliente { IdCliente = 1 };
            var fechaInicio = DateTime.Today.AddDays(3);
            var fechaFin = fechaInicio.AddDays(2);

            var result = _service.ReservarHabitacion(habitacion, cliente, fechaInicio, fechaFin);

            Assert.IsFalse(result);
        }

        [Test]
        public void TC107_CancelarReserva_Activa_Mayor24h()
        {
            var reserva = new Reserva
            {
                Estado = "Activa",
                FechaInicio = DateTime.Now.AddDays(2)
            };

            var result = _service.CancelarReserva(reserva);

            Assert.IsTrue(result);
        }

        [Test]
        public void TC108_CancelarReserva_Finalizada_Error()
        {
            var reserva = new Reserva
            {
                Estado = "Finalizada",
                FechaInicio = DateTime.Now.AddDays(-3),
                FechaFin = DateTime.Now.AddDays(-1)
            };

            var result = _service.CancelarReserva(reserva);

            Assert.IsFalse(result);
        }

        [Test]
        public void TC113_EnviarNotificacion_CheckIn24h()
        {
            var reserva = new Reserva
            {
                Estado = "Activa",
                FechaInicio = DateTime.Now.AddHours(23)
            };

            var result = _service.EnviarNotificacionCheckIn(reserva);

            Assert.IsTrue(result);
        }

        [Test]
        public void TC114_EnviarNotificacion_ReservaCancelada()
        {
            var reserva = new Reserva
            {
                Estado = "Cancelada",
                FechaInicio = DateTime.Now.AddHours(23)
            };

            var result = _service.EnviarNotificacionCheckIn(reserva);

            Assert.IsFalse(result);
        }

        [Test]
        public void TC117_GenerarFactura_CheckOutConPago()
        {
            var reserva = new Reserva
            {
                Estado = "Finalizada",
                Pagado = true,
                FechaFin = DateTime.Now
            };

            var factura = _service.GenerarFactura(reserva);

            Assert.IsNotNull(factura);
            Assert.AreEqual(reserva.IdReserva, factura.IdReserva);
        }

        [Test]
        public void TC118_GenerarFactura_SinPago_Error()
        {
            var reserva = new Reserva
            {
                Estado = "Finalizada",
                Pagado = false,
                FechaFin = DateTime.Now
            };

            var factura = _service.GenerarFactura(reserva);

            Assert.IsNull(factura);
        }
    }

    // Servicio simulado para encapsular la lógica de negocio de reservas
    public class ReservaService
    {
        private readonly DbEntities _context;

        public ReservaService(DbEntities context)
        {
            _context = context;
        }

        public bool ReservarHabitacion(Habitacion habitacion, Cliente cliente, DateTime inicio, DateTime fin)
        {
            if (habitacion.Estado != "Disponible") return false;
            if (inicio >= fin) return false;

            // Simulación exitosa
            return true;
        }

        public bool CancelarReserva(Reserva reserva)
        {
            if (reserva.Estado == "Finalizada" || reserva.FechaInicio <= DateTime.Now)
                return false;

            // Simulación exitosa
            return true;
        }

        public bool EnviarNotificacionCheckIn(Reserva reserva)
        {
            if (reserva.Estado == "Cancelada") return false;

            TimeSpan diff = reserva.FechaInicio - DateTime.Now;
            return diff.TotalHours <= 24;
        }

        public Factura GenerarFactura(Reserva reserva)
        {
            if (!reserva.Pagado) return null;

            return new Factura
            {
                IdFactura = 1,
                IdReserva = reserva.IdReserva,
                Fecha = DateTime.Now,
                Monto = 100 // simulado
            };
        }
    }

    public class Factura
    {
        public int IdFactura { get; set; }
        public int IdReserva { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
    }
}
