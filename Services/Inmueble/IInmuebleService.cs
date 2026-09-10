using InmobiliariaTPI.Models;
using X.PagedList;

namespace InmobiliariaTPI.Services
{
    public interface IInmuebleService : IBaseService<Inmueble>
    {
        Task<IPagedList<Inmueble>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null, bool soloDisponibles = false);
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