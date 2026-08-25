using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using X.PagedList;
using X.PagedList.Extensions;

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
            if (string.IsNullOrWhiteSpace(inquilino.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            var existente = await _repository.GetByIdAsync(inquilino.Id);
            if (existente == null)
                throw new InvalidOperationException("Inquilino no encontrado");

            if (existente.Dni != inquilino.Dni && await _repository.ExisteDniAsync(inquilino.Dni))
                throw new InvalidOperationException("El DNI ya está registrado por otro inquilino");

            await _repository.UpdateAsync(inquilino);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IPagedList<Inquilino>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var inquilinos = await _repository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                inquilinos = inquilinos.Where(p =>
                    p.NombreCompleto!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Dni!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    p.Email!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return inquilinos.ToPagedList(pageNumber, pageSize);
        }
    }
}