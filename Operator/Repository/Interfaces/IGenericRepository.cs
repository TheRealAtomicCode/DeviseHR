using OP.DTO;

namespace OP.Repository.Interfaces
{
    public interface IGenericRepository <T>
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByEmailAsync(string email);
        Task<ServiceResponse<IEnumerable<T>>> GetAllAsync(string? name, int pageNumber = 1, int pageSize = 10);
        Task AddAsync(T opj);
        Task DeleteAsync(T opj);
    }
}
