using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Services
{
    public interface IReservaService : IBaseService<Reserva>
    {
        Task<IEnumerable<Reserva>> GetVigentesAsync();
        Task<IEnumerable<Reserva>> GetPorTerminarAsync(int dias);
        Task<IEnumerable<Reserva>> GetPorInmuebleAsync(int inmuebleId);
        Task<IEnumerable<Reserva>> GetPorInquilinoAsync(int inquilinoId);
        Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin);
        Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin, int? reservaExcluirId); // ← AGREGAR ESTO
        Task FinalizarAsync(int id, DateTime fechaTerminacion, int idUsuarioTerminacion);
        Task<Reserva> RenovarAsync(Reserva nuevaReserva);
    }
}