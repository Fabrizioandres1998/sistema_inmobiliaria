using InmobiliariaTPI.Models;
using X.PagedList;

namespace InmobiliariaTPI.Services
{
        public interface IBaseService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IPagedList<T>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm = null);
    }
}