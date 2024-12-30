using HR.DTO;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IWorkingPatternRepo
    {
        Task AddAsync(WorkingPattern pattern);
        Task<List<WorkingPatternDto>> GetAllWorkingPatternsByName(string? searchTerm, int? page, int? skip, int companyId);
        Task<WorkingPatternDto> GetWorkingPatternByIdOrDefault(int patternId, int companyId);
        Task SaveChangesAsync();
    }
}
