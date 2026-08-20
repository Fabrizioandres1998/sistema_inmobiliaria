using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Repositories
{
    public interface IPropietarioRepository
    {
        Task<IEnumerable<Propietario>> GetAllAsync();
        Task<Propietario?> GetByIdAsync(int id);
        Task<int> CreateAsync(Propietario propietario);
        Task UpdateAsync(Propietario propietario);
        Task DeleteAsync(int id);
        Task<bool> ExisteDniAsync(string dni);
    }
}
