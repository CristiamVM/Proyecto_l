using ReservasHoteles.Entidades;
using ReservasHoteles.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservasHoteles.Repositorios
{
    public class RepositorioReservas : IRepositorioReservas
    {
        private readonly List<Reserva> reservas = new();

        public void Agregar(Reserva reserva) => reservas.Add(reserva);

        public List<Reserva> ObtenerTodas() => reservas;

        public Reserva ObtenerPorId(int id) =>
            reservas.FirstOrDefault(r => r.Id == id);
    }

}
