using InmobiliariaTPI.Data;
using InmobiliariaTPI.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;

namespace InmobiliariaTPI.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DatabaseHelper _dbHelper;
        protected readonly ILogger<T> _logger;

        public BaseRepository(DatabaseHelper dbHelper, ILogger<T> logger)
        {
            _dbHelper = dbHelper;
            _logger = logger;
        }

        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<T?> GetByIdAsync(int id);
        
        public virtual async Task<int> CreateAsync(T entity)
        {
            throw new NotImplementedException("Este método debe ser sobrescrito en el repositorio concreto");
        }

        public virtual async Task UpdateAsync(T entity)
        {
            throw new NotImplementedException("Este método debe ser sobrescrito en el repositorio concreto");
        }

        public virtual async Task DeleteAsync(int id)
        {
            throw new NotImplementedException("Este método debe ser sobrescrito en el repositorio concreto");
        }
    }
}