using Models;

namespace HR.Repository.Interfaces
{
    public interface IWorkingPatternRepo
    {
        Task AddAsync(WorkingPattern pattern);
        Task SaveChangesAsync();
    }
}
