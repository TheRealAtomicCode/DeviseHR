using HR.DTO;

namespace HR.Services.Interfaces
{
    public interface IAbsenceService
    {
        Task<AbsenceDto> AddOrRequestAbsence(AddAbsenceRequest absenceRequest, int myId, int companyId, int userRole);
    }
}
