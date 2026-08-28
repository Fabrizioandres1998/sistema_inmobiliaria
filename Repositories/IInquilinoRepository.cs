using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Repositories
{
    public interface IInquilinoRepository
    {
        Task<IEnumerable<Inquilino>> GetAllAsync();
        Task<Inquilino?> GetByIdAsync(int id);
        Task<int> CreateAsync(Inquilino inquilino);
        Task UpdateAsync(Inquilino inquilino);
        Task DeleteAsync(int id);
        Task<bool> ExisteDniAsync(string dni);
    }
}
