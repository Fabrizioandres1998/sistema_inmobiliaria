using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Repositories
{
    public interface IInmuebleRepository : IBaseRepository<Inmueble>
    {
        Task<IEnumerable<Inmueble>> GetByPropietarioIdAsync(int propietarioId);
        Task<IEnumerable<Inmueble>> GetDisponiblesAsync();
        Task<IEnumerable<Inmueble>> GetDisponiblesEnFechasAsync(DateTime inicio, DateTime fin);
        Task<IEnumerable<Inmueble>> GetMasReservadosAsync(int dias);
        Task<IEnumerable<Inmueble>> GetSinReservasAsync(int dias);
        Task<bool> EstaDisponibleEnFechasAsync(int inmuebleId, DateTime inicio, DateTime fin);
        Task<bool> ExisteDireccionAsync(string direccion);
        Task SuspenderAsync(int id);
        Task ActivarAsync(int id);
    }
}