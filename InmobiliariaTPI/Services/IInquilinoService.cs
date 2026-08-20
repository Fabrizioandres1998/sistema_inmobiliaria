 using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Services
{
    public interface IInquilinoService
    {
        Task<IEnumerable<Inquilino>> GetAllAsync();
        Task<Inquilino?> GetByIdAsync(int id);
        Task<Inquilino> CreateAsync(Inquilino inquilino);
        Task UpdateAsync(Inquilino inquilino);
        Task DeleteAsync(int id);
    }
}
