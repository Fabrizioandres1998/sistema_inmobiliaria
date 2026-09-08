namespace InmobiliariaTPI.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<int> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize, string? searchTerm = null);
        Task<int> GetTotalCountAsync(string? searchTerm = null);
    }
}