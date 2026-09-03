using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Services
{
    public class InmuebleService : BaseService<Inmueble, IInmuebleRepository>, IInmuebleService
    {
        public InmuebleService(IInmuebleRepository repository, ILogger<Inmueble> logger)
            : base(repository, logger)
        {
        }

        // metodos especificos que pasan directamente al repositorio
        public async Task<IEnumerable<Inmueble>> GetByPropietarioIdAsync(int propietarioId)
        {
            _logger.LogInformation("Obteniendo inmuebles del propietario {PropietarioId}", propietarioId);
            return await _repository.GetByPropietarioIdAsync(propietarioId);
        }

        public async Task<IEnumerable<Inmueble>> GetDisponiblesAsync()
        {
            _logger.LogInformation("Obteniendo inmuebles disponibles");
            return await _repository.GetDisponiblesAsync();
        }

        public async Task<IEnumerable<Inmueble>> GetDisponiblesEnFechasAsync(DateTime inicio, DateTime fin)
        {
            _logger.LogInformation("Obteniendo inmuebles disponibles entre {Inicio} y {Fin}", inicio, fin);
            return await _repository.GetDisponiblesEnFechasAsync(inicio, fin);
        }

        public async Task<IEnumerable<Inmueble>> GetMasReservadosAsync(int dias)
        {
            _logger.LogInformation("Obteniendo inmuebles mas reservados en los ultimos {Dias} dias", dias);
            return await _repository.GetMasReservadosAsync(dias);
        }

        public async Task<IEnumerable<Inmueble>> GetSinReservasAsync(int dias)
        {
            _logger.LogInformation("Obteniendo inmuebles sin reservas en los ultimos {Dias} dias", dias);
            return await _repository.GetSinReservasAsync(dias);
        }

        public async Task<bool> EstaDisponibleEnFechasAsync(int inmuebleId, DateTime inicio, DateTime fin)
        {
            _logger.LogInformation("Verificando disponibilidad del inmueble {InmuebleId}", inmuebleId);
            return await _repository.EstaDisponibleEnFechasAsync(inmuebleId, inicio, fin);
        }

        public async Task<bool> ExisteDireccionAsync(string direccion)
        {
            _logger.LogInformation("Verificando si existe la direccion {Direccion}", direccion);
            return await _repository.ExisteDireccionAsync(direccion);
        }

        public async Task SuspenderAsync(int id)
        {
            _logger.LogInformation("Suspendiendo inmueble {Id}", id);
            await _repository.SuspenderAsync(id);
        }

        public async Task ActivarAsync(int id)
        {
            _logger.LogInformation("Activando inmueble {Id}", id);
            await _repository.ActivarAsync(id);
        }

        // implementacion del metodo abstracto SearchAsync para el paginado
        protected override async Task<IEnumerable<Inmueble>> SearchAsync(IEnumerable<Inmueble> items, string searchTerm)
        {
            return items.Where(i =>
                i.Direccion!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                i.Coordenadas!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }

        // sobrescribo create para validar direccion unica
        public override async Task<Inmueble> CreateAsync(Inmueble inmueble)
        {
            _logger.LogInformation("Creando nuevo inmueble - Direccion: {Direccion}", inmueble.Direccion);

            // valido que no exista otra direccion igual
            if (await _repository.ExisteDireccionAsync(inmueble.Direccion!))
                throw new InvalidOperationException("Ya existe un inmueble con esa direccion");

            // seteo fecha de creacion si no viene
            if (inmueble.FechaCreacion == default)
                inmueble.FechaCreacion = DateTime.Now;

            // por defecto disponible = true
            inmueble.Disponible = true;

            return await base.CreateAsync(inmueble);
        }

        // sobrescribo update para validar direccion unica (excepto el mismo)
        public override async Task UpdateAsync(Inmueble inmueble)
        {
            _logger.LogInformation("Actualizando inmueble {Id}", inmueble.Id);

            // obtengo el inmueble actual para comparar direccion
            var existente = await _repository.GetByIdAsync(inmueble.Id);
            if (existente == null)
                throw new InvalidOperationException("Inmueble no encontrado");

            // si cambio la direccion, valido que no este usada por otro
            if (existente.Direccion != inmueble.Direccion &&
                await _repository.ExisteDireccionAsync(inmueble.Direccion!))
                throw new InvalidOperationException("Ya existe otro inmueble con esa direccion");

            await base.UpdateAsync(inmueble);
        }
    }
}