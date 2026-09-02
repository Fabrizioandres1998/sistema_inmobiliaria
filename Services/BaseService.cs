using InmobiliariaTPI.Repositories;
using Microsoft.Extensions.Logging;
using X.PagedList;
using X.PagedList.Extensions;

namespace InmobiliariaTPI.Services
{
    public abstract class BaseService<TEntity, TRepository> : IBaseService<TEntity>
        where TEntity : class
        where TRepository : IBaseRepository<TEntity>
    {
        protected readonly TRepository _repository;
        protected readonly ILogger<TEntity> _logger;

        public BaseService(TRepository repository, ILogger<TEntity> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los {Entity}", typeof(TEntity).Name);
            return await _repository.GetAllAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo {Entity} por ID: {Id}", typeof(TEntity).Name, id);
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entidad)
        {
            _logger.LogInformation("Creando nueva {Entity}", typeof(TEntity).Name);
            await _repository.CreateAsync(entidad);
            return entidad;
        }

        public virtual async Task UpdateAsync(TEntity entidad)
        {
            _logger.LogInformation("Actualizando {Entity}", typeof(TEntity).Name);
            await _repository.UpdateAsync(entidad);
        }

        public virtual async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Eliminando {Entity} ID: {Id}", typeof(TEntity).Name, id);
            await _repository.DeleteAsync(id);
        }

        public virtual async Task<IPagedList<TEntity>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            _logger.LogInformation("Obteniendo página {Page} de {Entity}", pageNumber, typeof(TEntity).Name);
            var items = await _repository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                items = await SearchAsync(items, searchTerm);
            }

            return items.ToPagedList(pageNumber, pageSize);
        }

        protected abstract Task<IEnumerable<TEntity>> SearchAsync(IEnumerable<TEntity> items, string searchTerm);
    }
}