using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Services
{
    public interface IPropietarioService
    {
        Task<IEnumerable<Propietario>> GetAllAsync();
        Task<Propietario?> GetByIdAsync(int id);
        Task<Propietario> CreateAsync(Propietario propietario);
        Task UpdateAsync(Propietario propietario);
        Task DeleteAsync(int id);
    }
}
