using InmobiliariaTPI.Models;
using X.PagedList;

namespace InmobiliariaTPI.Services
{
    public interface IPropietarioService
    {
        Task<IEnumerable<Propietario>> GetAllAsync();
        Task<Propietario?> GetByIdAsync(int id);
        Task<Propietario> CreateAsync(Propietario propietario);
        Task UpdateAsync(Propietario propietario);
        Task DeleteAsync(int id);
        Task<IPagedList<Propietario>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
    }
}
