using NUnit.Framework;
using Moq;
using ReservasHoteles.Entidades;
using ReservasHoteles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReservasHotelesTest.Tests
{
    public class HabitacionServiceTests
    {
        private Mock<IRepositorioHabitaciones> _mockRepo;
        private HabitacionService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepositorioHabitaciones>();
            _service = new HabitacionService(_mockRepo.Object);
        }

        [Test]
        public void TC101_RegistrarHabitacion_ConDatosValidos()
        {
            // Arrange
            var nuevaHabitacion = new Habitacion
            {
                Numero = "A101",
                Tipo = "Suite",
                Precio = 150.00m,
                Estado = "Disponible"
            };

            // Act
            var resultado = _service.RegistrarHabitacion(nuevaHabitacion);

            // Assert
            Assert.IsTrue(resultado, "La habitación con datos válidos debe registrarse correctamente.");
            _mockRepo.Verify(r => r.Agregar(It.Is<Habitacion>(h => h.Numero == "A101")), Times.Once);
        }

        [Test]
        public void TC102_RegistrarHabitacion_SinNumero_Error()
        {
            // Arrange
            var habitacionInvalida = new Habitacion
            {
                Numero = null,
                Tipo = "Suite",
                Precio = 100,
                Estado = "Disponible"
            };

            // Act
            var resultado = _service.RegistrarHabitacion(habitacionInvalida);

            // Assert
            Assert.IsFalse(resultado, "Una habitación sin número no debe registrarse.");
            _mockRepo.Verify(r => r.Agregar(It.IsAny<Habitacion>()), Times.Never);
        }

        [Test]
        public void TC103_BuscarHabitaciones_TipoSuite_Disponible()
        {
            // Arrange
            var habitaciones = new List<Habitacion>
            {
                new Habitacion { Numero = "S1", Tipo = "Suite", Estado = "Disponible" },
                new Habitacion { Numero = "S2", Tipo = "Suite", Estado = "Ocupado" },
                new Habitacion { Numero = "E1", Tipo = "Estándar", Estado = "Disponible" }
            };

            _mockRepo.Setup(r => r.ObtenerTodas()).Returns(habitaciones);

            // Act
            var resultado = _service.BuscarHabitaciones("Suite", "Disponible");

            // Assert
            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("S1", resultado[0].Numero);
        }

        [Test]
        public void TC104_BuscarHabitaciones_SinFiltros_Error()
        {
            // Arrange - No se necesita

            // Act
            var resultado = _service.BuscarHabitaciones(null, null);

            // Assert
            Assert.IsNull(resultado, "Sin filtros, la búsqueda debe retornar null.");
        }

        [Test]
        public void TC115_ConsultarDisponibilidad_ConFechasValidas()
        {
            // Arrange
            DateTime checkIn = DateTime.Today.AddDays(5);
            DateTime checkOut = DateTime.Today.AddDays(7);

            // Act
            var resultado = _service.ValidarFechasDisponibilidad(checkIn, checkOut);

            // Assert
            Assert.IsTrue(resultado, "La fecha de check-in debe ser anterior a la de check-out.");
        }

        [Test]
        public void TC116_ConsultarDisponibilidad_FechasInvalidas()
        {
            // Arrange
            DateTime checkIn = DateTime.Today.AddDays(5);
            DateTime checkOut = DateTime.Today.AddDays(3);

            // Act
            var resultado = _service.ValidarFechasDisponibilidad(checkIn, checkOut);

            // Assert
            Assert.IsFalse(resultado, "El check-in no puede ser posterior al check-out.");
        }
    }
}
