using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using X.PagedList;
using X.PagedList.Extensions;

namespace InmobiliariaTPI.Services
{
    public class PropietarioService : IPropietarioService
    {
        private readonly IPropietarioRepository _repository;

        public PropietarioService(IPropietarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Propietario>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Propietario?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Propietario> CreateAsync(Propietario propietario)
        {
            if (string.IsNullOrWhiteSpace(propietario.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            if (await _repository.ExisteDniAsync(propietario.Dni))
                throw new InvalidOperationException("El DNI ya está registrado");

            var id = await _repository.CreateAsync(propietario);
            propietario.Id = id;
            return propietario;
        }

        public async Task UpdateAsync(Propietario propietario)
        {
            if (string.IsNullOrWhiteSpace(propietario.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            await _repository.UpdateAsync(propietario);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IPagedList<Propietario>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var propietarios = await _repository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                propietarios = propietarios.Where(p =>
                    p.NombreCompleto!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Dni!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Email!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return propietarios.ToPagedList(pageNumber, pageSize);
        }
    }
}