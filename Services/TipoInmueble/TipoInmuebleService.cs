using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Services
{
    public class TipoInmuebleService : BaseService<TipoInmueble, ITipoInmuebleRepository>, ITipoInmuebleService
    {
        public TipoInmuebleService(ITipoInmuebleRepository repository, ILogger<TipoInmueble> logger)
            : base(repository, logger)
        {
        }

        // implemento el metodo abstracto SearchAsync para el paginado
        protected override async Task<IEnumerable<TipoInmueble>> SearchAsync(IEnumerable<TipoInmueble> items, string searchTerm)
        {
            return items.Where(t =>
                t.Nombre!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                (t.Descripcion != null && t.Descripcion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            );
        }

        // sobrescribo create para validar que el nombre no este duplicado
        public override async Task<TipoInmueble> CreateAsync(TipoInmueble tipo)
        {
            _logger.LogInformation("Creando nuevo tipo de inmueble - Nombre: {Nombre}", tipo.Nombre);

            // valido que no exista otro tipo con el mismo nombre
            var todos = await _repository.GetAllAsync();
            if (todos.Any(t => t.Nombre!.Equals(tipo.Nombre, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Ya existe un tipo de inmueble con ese nombre");

            return await base.CreateAsync(tipo);
        }

        // sobrescribo update para validar nombre unico (excepto el mismo)
        public override async Task UpdateAsync(TipoInmueble tipo)
        {
            _logger.LogInformation("Actualizando tipo de inmueble {Id}", tipo.Id);

            // obtengo el tipo actual
            var existente = await _repository.GetByIdAsync(tipo.Id);
            if (existente == null)
                throw new InvalidOperationException("Tipo de inmueble no encontrado");

            // si cambio el nombre, valido que no este usado por otro
            if (existente.Nombre != tipo.Nombre)
            {
                var todos = await _repository.GetAllAsync();
                if (todos.Any(t => t.Id != tipo.Id &&
                                t.Nombre!.Equals(tipo.Nombre, StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException("Ya existe otro tipo de inmueble con ese nombre");
            }

            await base.UpdateAsync(tipo);
        }
    }
}