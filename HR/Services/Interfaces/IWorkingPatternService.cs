using HR.DTO;

namespace HR.Services.Interfaces
{
    public interface IWorkingPatternService
    {
        Task<int> AddWorkingPattern(WorkingPatternRequest workigPatternRequest, int myId, int companyId);
        Task<List<WorkingPatternDto>> GetAllWorkingPatterns(string? searchTerm, int? page, int? skip, int companyId);
    }
}
