using HR.DTO;
using HR.DTO.Inbound;

namespace HR.Services.Interfaces
{
    public interface IAbsenceService
    {
        Task<AbsenceDto> AddOrRequestAbsence(AddAbsenceRequest absenceRequest, int myId, int companyId, int userRole);
    }
}
