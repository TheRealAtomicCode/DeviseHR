using HR.DTO;

namespace HR.Services.Interfaces
{
    public interface IWorkingPatternService
    {
        Task<int> AddWorkingPattern(WorkingPatternRequest workigPatternRequest, int myId, int companyId);
    }
}
