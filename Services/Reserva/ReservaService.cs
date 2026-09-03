using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Services
{
    public class ReservaService : BaseService<Reserva, IReservaRepository>, IReservaService
    {
        public ReservaService(IReservaRepository repository, ILogger<Reserva> logger)
            : base(repository, logger)
        {
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
            _logger.LogInformation("Verificando disponibilidad del inmueble {InmuebleId}", inmuebleId);
            return await _repository.EstaOcupadoAsync(inmuebleId, inicio, fin);
        }

        public async Task FinalizarAsync(int id, DateTime fechaTerminacion, int idUsuarioTerminacion)
        {
            _logger.LogInformation("Finalizando reserva {Id}", id);
            var reserva = await _repository.GetByIdAsync(id);
            if (reserva == null)
                throw new InvalidOperationException("Reserva no encontrada");

            if (reserva.Estado != "Activa")
                throw new InvalidOperationException("La reserva no esta activa");

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

            // validar que la fecha de inicio sea menor a la de fin
            if (reserva.FechaInicio >= reserva.FechaFin)
                throw new InvalidOperationException("La fecha de inicio debe ser menor a la fecha de fin");

            // validar que el inmueble no este ocupado
            if (await _repository.EstaOcupadoAsync(reserva.IdInmueble, reserva.FechaInicio, reserva.FechaFin))
                throw new InvalidOperationException("El inmueble no esta disponible en esas fechas");

            reserva.Estado = "Activa";
            reserva.FechaCreacion = DateTime.Now;

            return await base.CreateAsync(reserva);
        }

        protected override async Task<IEnumerable<Reserva>> SearchAsync(IEnumerable<Reserva> items, string searchTerm)
        {
            return items.Where(r =>
                r.Id.ToString().Contains(searchTerm) ||
                r.Inquilino!.NombreCompleto!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                r.Inmueble!.Direccion!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
