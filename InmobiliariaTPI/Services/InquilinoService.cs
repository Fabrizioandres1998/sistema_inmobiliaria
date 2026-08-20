using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;

namespace InmobiliariaTPI.Services
{
    public class InquilinoService : IInquilinoService
    {
        private readonly IInquilinoRepository _repository;

        public InquilinoService(IInquilinoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Inquilino>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Inquilino?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Inquilino> CreateAsync(Inquilino inquilino)
        {
            if (string.IsNullOrWhiteSpace(inquilino.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            if (await _repository.ExisteDniAsync(inquilino.Dni))
                throw new InvalidOperationException("El DNI ya está registrado");

            var id = await _repository.CreateAsync(inquilino);
            inquilino.Id = id;
            return inquilino;
        }

        public async Task UpdateAsync(Inquilino inquilino)
        {
            await _repository.UpdateAsync(inquilino);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}