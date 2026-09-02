using InmobiliariaTPI.Models;

namespace InmobiliariaTPI.Repositories
{
    public interface IPropietarioRepository : IBaseRepository<Propietario>
    {
        Task<bool> ExisteDniAsync(string dni);
    }
}
