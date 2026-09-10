using InmobiliariaTPI.Models;
using X.PagedList;

namespace InmobiliariaTPI.Services
{
    public interface IReservaService : IBaseService<Reserva>
    {
        Task<IEnumerable<Reserva>> GetVigentesAsync();
        Task<IEnumerable<Reserva>> GetPorTerminarAsync(int dias);
        Task<IEnumerable<Reserva>> GetPorInmuebleAsync(int inmuebleId);
        Task<IEnumerable<Reserva>> GetPorInquilinoAsync(int inquilinoId);
        Task<IPagedList<Reserva>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, bool soloVigentes = false);
        Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin);
        Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin, int? reservaExcluirId); // ← AGREGAR ESTO
        Task FinalizarAsync(int id, DateTime fechaTerminacion, int idUsuarioTerminacion);
        Task<Reserva> RenovarAsync(Reserva nuevaReserva);
    }
}