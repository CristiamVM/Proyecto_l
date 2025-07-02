using ReservasHoteles.Logica;
using ReservasHoteles.Repositorios;
using System;

class Program
{
    static void Main()
    {

        var hotelAdmin = new HotelAdmin(
         new RepositorioHabitaciones(),
         new RepositorioUsuarios(),
         new RepositorioReservas()
        );
    }
}
