using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;
using X.PagedList;

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

            await ValidatePropietarioAsync(propietario, isNew: true);

            return await base.CreateAsync(propietario);
        }

        public override async Task UpdateAsync(Propietario propietario)
        {
            _logger.LogInformation("Actualizando propietario ID: {Id}", propietario.Id);

            var existing = await _repository.GetByIdAsync(propietario.Id);
            if (existing == null)
                throw new NotFoundException($"Propietario con ID {propietario.Id} no encontrado");

            await ValidatePropietarioAsync(propietario, isNew: false);

            await base.UpdateAsync(propietario);
        }

        private async Task ValidatePropietarioAsync(Propietario propietario, bool isNew)
        {
            ValidateFormat(propietario);
            await ValidateBusinessRules(propietario, isNew);
        }

        private void ValidateFormat(Propietario propietario)
        {
            if (string.IsNullOrWhiteSpace(propietario.Dni))
                throw new ValidationException("El DNI es obligatorio");

            if (propietario.Dni.Length < 7 || propietario.Dni.Length > 10)
                throw new ValidationException("El DNI debe tener entre 7 y 10 caracteres");

            if (string.IsNullOrWhiteSpace(propietario.NombreCompleto))
                throw new ValidationException("El nombre completo es obligatorio");
        }

        private async Task ValidateBusinessRules(Propietario propietario, bool isNew)
        {
            if (isNew)
            {
                if (!string.IsNullOrWhiteSpace(propietario.Dni) && await _repository.ExisteDniAsync(propietario.Dni))
                    throw new BusinessRuleException("El DNI ya está registrado");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(propietario.Dni))
                {
                    var existingWithDni = await _repository.GetByDniAsync(propietario.Dni);
                    if (existingWithDni != null && existingWithDni.Id != propietario.Id)
                        throw new BusinessRuleException("El DNI ya está registrado por otro propietario");
                }
            }
        }

        // sobrescribo GetPagedAsync para usar paginacion en base de datos
        public override async Task<IPagedList<Propietario>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo página {Page} de propietarios", pageNumber);

            var items = await _repository.GetPagedAsync(pageNumber, pageSize, searchTerm);
            var totalCount = await _repository.GetTotalCountAsync(searchTerm);

            return new StaticPagedList<Propietario>(items, pageNumber, pageSize, totalCount);
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

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}

public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}