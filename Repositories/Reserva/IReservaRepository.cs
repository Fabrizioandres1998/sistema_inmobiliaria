using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Repositories
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        // obtiene reservas vigentes (fecha actual entre fecha_inicio y fecha_fin)
        Task<IEnumerable<Reserva>> GetVigentesAsync();

        // obtiene reservas que terminan en X dias
        Task<IEnumerable<Reserva>> GetPorTerminarAsync(int dias);

        // obtiene reservas de un inmueble
        Task<IEnumerable<Reserva>> GetPorInmuebleAsync(int inmuebleId);

        // obtiene reservas de un inquilino
        Task<IEnumerable<Reserva>> GetPorInquilinoAsync(int inquilinoId);

        // verifica si un inmueble esta ocupado entre dos fechas
        Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin);

        // finaliza una reserva (con multa)
        Task FinalizarAsync(int id, DateTime fechaTerminacion, decimal? multa, int idUsuarioTerminacion);

        // renueva una reserva (crea una nueva reserva)
        Task<Reserva> RenovarAsync(Reserva nuevaReserva);
    }
}
