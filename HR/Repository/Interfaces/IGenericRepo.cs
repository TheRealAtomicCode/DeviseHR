using System.Linq.Expressions;

namespace HR.Repository.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task CreateAsync(T entity);
        Task SaveChangesAsync();
    }
}