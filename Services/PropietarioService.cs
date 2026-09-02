using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;
using X.PagedList;
using X.PagedList.Extensions;

namespace InmobiliariaTPI.Services
{
    public class PropietarioService : BaseService<Propietario, IPropietarioRepository>, IPropietarioService
    {
        public PropietarioService(IPropietarioRepository repository, ILogger<Propietario> logger) 
            : base(repository, logger)
        {
        }

        public override async Task<Propietario> CreateAsync(Propietario propietario)
        {
            _logger.LogInformation("Creando nuevo propietario - DNI: {Dni}", propietario.Dni);

            if (string.IsNullOrWhiteSpace(propietario.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            if (await _repository.ExisteDniAsync(propietario.Dni))
                throw new InvalidOperationException("El DNI ya está registrado");

            return await base.CreateAsync(propietario);
        }

        public override async Task UpdateAsync(Propietario propietario)
        {
            _logger.LogInformation("Actualizando propietario ID: {Id}", propietario.Id);

            if (string.IsNullOrWhiteSpace(propietario.Dni))
                throw new ArgumentException("El DNI es obligatorio");

            await base.UpdateAsync(propietario);
        }

        protected override async Task<IEnumerable<Propietario>> SearchAsync(IEnumerable<Propietario> items, string searchTerm)
        {
            return items.Where(p =>
                p.NombreCompleto!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Dni!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Email!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}