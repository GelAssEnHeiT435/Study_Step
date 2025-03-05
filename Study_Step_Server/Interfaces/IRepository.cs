namespace Study_Step_Server.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id); // Select operation
        Task AddAsync(T entity); // Insert operation
        Task UpdateAsync(T entity); // Update operation
        Task DeleteAsync(int id); // Delete operation
    }
}
