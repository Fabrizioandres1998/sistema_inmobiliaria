using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Repositories
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetVigentesAsync();
        Task<IEnumerable<Reserva>> GetPorTerminarAsync(int dias);
        Task<IEnumerable<Reserva>> GetPorInmuebleAsync(int inmuebleId);
        Task<IEnumerable<Reserva>> GetPorInquilinoAsync(int inquilinoId);
        Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin);
        Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin, int? reservaExcluirId);
        Task FinalizarAsync(int id, DateTime fechaTerminacion, decimal? multa, int idUsuarioTerminacion);
        Task<Reserva> RenovarAsync(Reserva nuevaReserva);
    }
}