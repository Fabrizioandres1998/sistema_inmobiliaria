
using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;

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
            await _repository.UpdateAsync(propietario);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}