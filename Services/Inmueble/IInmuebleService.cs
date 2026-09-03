using InmobiliariaTPI.Models;
using X.PagedList;

namespace InmobiliariaTPI.Services
{
    public interface IInmuebleService : IBaseService<Inmueble>
    {
        // busquedas especificas
        Task<IEnumerable<Inmueble>> GetByPropietarioIdAsync(int propietarioId);
        Task<IEnumerable<Inmueble>> GetDisponiblesAsync();
        Task<IEnumerable<Inmueble>> GetDisponiblesEnFechasAsync(DateTime inicio, DateTime fin);
        
        // informes
        Task<IEnumerable<Inmueble>> GetMasReservadosAsync(int dias);
        Task<IEnumerable<Inmueble>> GetSinReservasAsync(int dias);
        
        // validaciones
        Task<bool> EstaDisponibleEnFechasAsync(int inmuebleId, DateTime inicio, DateTime fin);
        Task<bool> ExisteDireccionAsync(string direccion);
        
        // suspension
        Task SuspenderAsync(int id);
        Task ActivarAsync(int id);
    }
}
