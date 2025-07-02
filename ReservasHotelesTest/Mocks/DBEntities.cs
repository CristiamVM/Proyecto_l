using System.Data.Entity;
using Proyecto_l.Models;

namespace ReservasHotelesTest.Mocks
{
    public class DbEntities : DbContext
    {
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<Habitacion> Habitacion { get; set; }
    }
}
