using InmobiliariaTPI.Models;
using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace InmobiliariaTPI.Services
{
    public class PagoService : BaseService<Pago, IPagoRepository>, IPagoService
    {
        public PagoService(IPagoRepository repository, ILogger<Pago> logger)
            : base(repository, logger)
        {
        }

        public async Task<IEnumerable<Pago>> GetByReservaIdAsync(int reservaId)
        {
            _logger.LogInformation("Obteniendo pagos de la reserva ID: {ReservaId}", reservaId);
            return await _repository.GetByReservaIdAsync(reservaId);
        }

        public async Task<IEnumerable<Pago>> GetActivosByReservaIdAsync(int reservaId)
        {
            _logger.LogInformation("Obteniendo pagos activos de la reserva ID: {ReservaId}", reservaId);
            return await _repository.GetActivosByReservaIdAsync(reservaId);
        }

        public async Task AnularAsync(int id, int idUsuarioAnulacion)
        {
            _logger.LogInformation("Anulando pago ID: {Id}", id);

            var pago = await _repository.GetByIdAsync(id);
            if (pago == null)
                throw new InvalidOperationException("Pago no encontrado");

            if (pago.Estado == 0)
                throw new InvalidOperationException("El pago ya está anulado");

            await _repository.AnularAsync(id, idUsuarioAnulacion);
            _logger.LogInformation("Pago ID: {Id} anulado correctamente", id);
        }

        public override async Task<Pago> CreateAsync(Pago pago)
        {
            _logger.LogInformation("Creando nuevo pago - Concepto: {Concepto}", pago.Concepto);

            if (string.IsNullOrWhiteSpace(pago.Concepto))
                throw new ArgumentException("El concepto es obligatorio");

            if (pago.Importe <= 0)
                throw new ArgumentException("El importe debe ser mayor a 0");

            pago.Estado = 1; // Activo
            pago.FechaCreacion = DateTime.Now;

            return await base.CreateAsync(pago);
        }

        public override async Task UpdateAsync(Pago pago)
        {
            _logger.LogInformation("Actualizando pago ID: {Id}", pago.Id);

            var existente = await _repository.GetByIdAsync(pago.Id);
            if (existente == null)
                throw new InvalidOperationException("Pago no encontrado");

            if (existente.Estado == 0)
                throw new InvalidOperationException("No se puede editar un pago anulado");

            if (string.IsNullOrWhiteSpace(pago.Concepto))
                throw new ArgumentException("El concepto es obligatorio");

            // Solo se puede editar el concepto
            existente.Concepto = pago.Concepto;

            await _repository.UpdateAsync(existente);
            _logger.LogInformation("Pago ID: {Id} actualizado correctamente", pago.Id);
        }

        public override async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando pago ID: {Id}", id);
            throw new InvalidOperationException("Los pagos no se eliminan, se anulan");
        }

        public override async Task<IPagedList<Pago>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo página {Page} de pagos", pageNumber);

            var items = await _repository.GetPagedAsync(pageNumber, pageSize, searchTerm);
            var totalCount = await _repository.GetTotalCountAsync(searchTerm);

            return new StaticPagedList<Pago>(items, pageNumber, pageSize, totalCount);
        }

        protected override async Task<IEnumerable<Pago>> SearchAsync(IEnumerable<Pago> items, string searchTerm)
        {
            return items.Where(p =>
                p.Concepto.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}