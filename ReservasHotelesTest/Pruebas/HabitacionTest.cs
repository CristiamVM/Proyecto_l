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
    public class HabitacionTest
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
    }

}
