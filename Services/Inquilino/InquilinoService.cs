using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;
using X.PagedList;
using X.PagedList.Extensions;

namespace InmobiliariaTPI.Services
{
    public class InquilinoService : BaseService<Inquilino, IInquilinoRepository>, IInquilinoService
    {
        public InquilinoService(IInquilinoRepository repository, ILogger<Inquilino> logger)
            : base(repository, logger)
        {
        }

        public override async Task<Inquilino> CreateAsync(Inquilino inquilino)
        {
            _logger.LogInformation("Creando nuevo inquilino - DNI: {Dni}", inquilino.Dni);

            if (string.IsNullOrWhiteSpace(inquilino.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            if (await _repository.ExisteDniAsync(inquilino.Dni))
                throw new InvalidOperationException("El DNI ya está registrado");

            return await base.CreateAsync(inquilino);
        }

        public override async Task UpdateAsync(Inquilino inquilino)
        {
            _logger.LogInformation("Actualizando inquilino ID: {Id}", inquilino.Id);

            if (string.IsNullOrWhiteSpace(inquilino.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            var existente = await _repository.GetByIdAsync(inquilino.Id);
            if (existente == null)
                throw new InvalidOperationException("Inquilino no encontrado");

            if (existente.Dni != inquilino.Dni && await _repository.ExisteDniAsync(inquilino.Dni))
                throw new InvalidOperationException("El DNI ya está registrado por otro inquilino");

            await base.UpdateAsync(inquilino);
        }

        protected override async Task<IEnumerable<Inquilino>> SearchAsync(IEnumerable<Inquilino> items, string searchTerm)
        {
            return items.Where(p =>
                p.NombreCompleto!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Dni!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Email!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}