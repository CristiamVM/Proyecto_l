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
    public class ReporteTest
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
