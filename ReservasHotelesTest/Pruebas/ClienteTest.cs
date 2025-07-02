using NUnit.Framework;
using Moq;
using Proyecto_l.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ReservasHotelesTest.Pruebas
{
    public class ClienteServiceTests
    {
        private Mock<DbEntities> _mockContext;
        private ClienteService _service;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<DbEntities>();
            _service = new ClienteService(_mockContext.Object);
        }

        [Test]
        public void TC109_RegistrarCliente_DatosValidos()
        {
            var cliente = new Cliente
            {
                Nombre = "Juan Pérez",
                Documento = "12345678",
                Correo = "juan@example.com"
            };

            var result = _service.RegistrarCliente(cliente);

            Assert.IsTrue(result);
        }

        [Test]
        public void TC110_RegistrarCliente_SinCorreo()
        {
            var cliente = new Cliente
            {
                Nombre = "Ana Torres",
                Documento = "98765432",
                Correo = null
            };

            var result = _service.RegistrarCliente(cliente);

            Assert.IsFalse(result);
        }

        [Test]
        public void TC111_VerHistorialReservas_ClienteConReservas()
        {
            var cliente = new Cliente { IdCliente = 1 };

            var reservas = new List<Reserva>
            {
                new Reserva { IdCliente = 1, Estado = "Finalizada" },
                new Reserva { IdCliente = 1, Estado = "Cancelada" },
                new Reserva { IdCliente = 2, Estado = "Finalizada" }
            }.AsQueryable();

            var mockSet = MockHelper.CreateMockDbSet(reservas);
            _mockContext.Setup(c => c.Reserva).Returns(mockSet.Object);

            var historial = _service.ObtenerHistorialReservas(cliente.IdCliente);

            Assert.AreEqual(2, historial.Count);
        }

        [Test]
        public void TC112_VerHistorialReservas_ClienteSinReservas()
        {
            var cliente = new Cliente { IdCliente = 3 };

            var reservas = new List<Reserva>
            {
                new Reserva { IdCliente = 1 },
                new Reserva { IdCliente = 2 }
            }.AsQueryable();

            var mockSet = MockHelper.CreateMockDbSet(reservas);
            _mockContext.Setup(c => c.Reserva).Returns(mockSet.Object);

            var historial = _service.ObtenerHistorialReservas(cliente.IdCliente);

            Assert.AreEqual(0, historial.Count);
        }
    }

    // Lógica simulada
    public class ClienteService
    {
        private readonly DbEntities _context;

        public ClienteService(DbEntities context)
        {
            _context = context;
        }

        public bool RegistrarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nombre) ||
                string.IsNullOrWhiteSpace(cliente.Documento) ||
                string.IsNullOrWhiteSpace(cliente.Correo))
            {
                return false;
            }

            return true;
        }

        public List<Reserva> ObtenerHistorialReservas(int idCliente)
        {
            return _context.Reserva
                .Where(r => r.IdCliente == idCliente)
                .ToList();
        }
    }

    public static class MockHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            return mockSet;
        }
    }
}