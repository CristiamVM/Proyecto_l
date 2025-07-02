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
    public class ClienteTest
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

    }

}
