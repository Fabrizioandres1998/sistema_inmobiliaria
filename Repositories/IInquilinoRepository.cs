using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Repositories
{
    public interface IInquilinoRepository : IBaseRepository<Inquilino>
    {
        Task<bool> ExisteDniAsync(string dni);
    }
}
