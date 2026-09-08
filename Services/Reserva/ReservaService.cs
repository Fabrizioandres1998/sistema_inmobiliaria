using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace InmobiliariaTPI.Services
{
    public class ReservaService : BaseService<Reserva, IReservaRepository>, IReservaService
    {
        private readonly IInmuebleService _inmuebleService;

        public ReservaService(
            IReservaRepository repository, 
            ILogger<Reserva> logger,
            IInmuebleService inmuebleService)
            : base(repository, logger)
        {
            _inmuebleService = inmuebleService;
        }

        public async Task<IEnumerable<Reserva>> GetVigentesAsync()
        {
            _logger.LogInformation("Obteniendo reservas vigentes");
            return await _repository.GetVigentesAsync();
        }

        public async Task<IEnumerable<Reserva>> GetPorTerminarAsync(int dias)
        {
            _logger.LogInformation("Obteniendo reservas que terminan en {Dias} dias", dias);
            return await _repository.GetPorTerminarAsync(dias);
        }

        public async Task<IEnumerable<Reserva>> GetPorInmuebleAsync(int inmuebleId)
        {
            _logger.LogInformation("Obteniendo reservas del inmueble {InmuebleId}", inmuebleId);
            return await _repository.GetPorInmuebleAsync(inmuebleId);
        }

        public async Task<IEnumerable<Reserva>> GetPorInquilinoAsync(int inquilinoId)
        {
            _logger.LogInformation("Obteniendo reservas del inquilino {InquilinoId}", inquilinoId);
            return await _repository.GetPorInquilinoAsync(inquilinoId);
        }

        public async Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin)
        {
            _logger.LogInformation("Verificando disponibilidad del inmueble {InmuebleId} entre {Inicio} y {Fin}", 
                inmuebleId, inicio, fin);
            return await _repository.EstaOcupadoAsync(inmuebleId, inicio, fin);
        }

        public async Task<bool> EstaOcupadoAsync(int inmuebleId, DateTime inicio, DateTime fin, int? reservaExcluirId)
        {
            _logger.LogInformation("Verificando disponibilidad del inmueble {InmuebleId} entre {Inicio} y {Fin} excluyendo reserva {ReservaExcluirId}", 
                inmuebleId, inicio, fin, reservaExcluirId);
            return await _repository.EstaOcupadoAsync(inmuebleId, inicio, fin, reservaExcluirId);
        }

        public async Task FinalizarAsync(int id, DateTime fechaTerminacion, int idUsuarioTerminacion)
        {
            _logger.LogInformation("Finalizando reserva {Id}", id);
            var reserva = await _repository.GetByIdAsync(id);
            if (reserva == null)
                throw new InvalidOperationException("Reserva no encontrada");

            if (reserva.Estado != "Activa")
                throw new InvalidOperationException("La reserva no esta activa");

            // validar que la fecha de terminacion no sea en el futuro
            if (fechaTerminacion.Date > DateTime.Now.Date)
                throw new InvalidOperationException("La fecha de terminacion no puede ser en el futuro");

            // calcular multa
            var diasOriginales = (reserva.FechaFin - reserva.FechaInicio).Days;
            var diasTranscurridos = (fechaTerminacion - reserva.FechaInicio).Days;
            var porcentaje = 0m;

            if (diasTranscurridos < diasOriginales / 2)
                porcentaje = 0.5m; // 50% si paso menos de la mitad
            else
                porcentaje = 0.25m; // 25% si paso mas de la mitad

            var multa = reserva.MontoPorDia * diasOriginales * porcentaje;

            await _repository.FinalizarAsync(id, fechaTerminacion, multa, idUsuarioTerminacion);
            _logger.LogInformation("Reserva {Id} finalizada con multa de {Multa}", id, multa);
        }

        public async Task<Reserva> RenovarAsync(Reserva nuevaReserva)
        {
            _logger.LogInformation("Renovando reserva para inmueble {InmuebleId}", nuevaReserva.IdInmueble);
            return await _repository.RenovarAsync(nuevaReserva);
        }

        // validaciones al crear reserva
        public override async Task<Reserva> CreateAsync(Reserva reserva)
        {
            _logger.LogInformation("Creando nueva reserva - Inmueble: {InmuebleId}", reserva.IdInmueble);

            await ValidateReservaAsync(reserva);

            // validar que el precio por día coincida con el del inmueble
            var inmueble = await _inmuebleService.GetByIdAsync(reserva.IdInmueble);
            if (inmueble == null)
                throw new InvalidOperationException("Inmueble no encontrado");
            
            if (reserva.MontoPorDia != inmueble.PrecioPorDia)
                throw new InvalidOperationException("El precio por día no coincide con el precio del inmueble");

            reserva.Estado = "Activa";
            reserva.FechaCreacion = DateTime.Now;

            return await base.CreateAsync(reserva);
        }

        // validaciones al editar reserva
        public override async Task UpdateAsync(Reserva reserva)
        {
            _logger.LogInformation("Actualizando reserva {Id}", reserva.Id);

            var existente = await _repository.GetByIdAsync(reserva.Id);
            if (existente == null)
                throw new InvalidOperationException("Reserva no encontrada");

            await ValidateReservaAsync(reserva, reserva.Id);

            await base.UpdateAsync(reserva);
        }

        // validaciones compartidas
        private async Task ValidateReservaAsync(Reserva reserva, int? reservaExcluirId = null)
        {
            // validar que la fecha de inicio sea menor a la de fin
            if (reserva.FechaInicio >= reserva.FechaFin)
                throw new InvalidOperationException("La fecha de inicio debe ser menor a la fecha de fin");

            // validar que la fecha de inicio no sea en el pasado
            if (reserva.FechaInicio.Date < DateTime.Now.Date)
                throw new InvalidOperationException("La fecha de inicio no puede ser en el pasado");

            // validar que la fecha de fin no sea en el pasado
            if (reserva.FechaFin.Date < DateTime.Now.Date)
                throw new InvalidOperationException("La fecha de fin no puede ser en el pasado");

            // validar que el inmueble no este ocupado (excluyendo la misma reserva si es edición)
            bool estaOcupado;
            if (reservaExcluirId.HasValue)
            {
                estaOcupado = await _repository.EstaOcupadoAsync(reserva.IdInmueble, reserva.FechaInicio, reserva.FechaFin, reservaExcluirId.Value);
            }
            else
            {
                estaOcupado = await _repository.EstaOcupadoAsync(reserva.IdInmueble, reserva.FechaInicio, reserva.FechaFin);
            }
            
            if (estaOcupado)
                throw new InvalidOperationException("El inmueble no esta disponible en esas fechas");
        }

        protected override async Task<IEnumerable<Reserva>> SearchAsync(IEnumerable<Reserva> items, string searchTerm)
        {
            return items.Where(r =>
                r.Id.ToString().Contains(searchTerm) ||
                r.Inquilino!.NombreCompleto!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                r.Inmueble!.Direccion!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }

        // sobrescribo GetPagedAsync para usar paginacion en base de datos
        public override async Task<IPagedList<Reserva>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo página {Page} de reservas", pageNumber);

            var items = await _repository.GetPagedAsync(pageNumber, pageSize, searchTerm);
            var totalCount = await _repository.GetTotalCountAsync(searchTerm);

            return new StaticPagedList<Reserva>(items, pageNumber, pageSize, totalCount);
        }
    }
}