using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Services
{
    public interface IPagoService : IBaseService<Pago>
    {
        Task<IEnumerable<Pago>> GetByReservaIdAsync(int reservaId);
        Task<IEnumerable<Pago>> GetActivosByReservaIdAsync(int reservaId);
        Task AnularAsync(int id, int idUsuarioAnulacion);
    }
}