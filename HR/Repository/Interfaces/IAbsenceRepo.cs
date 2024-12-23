using HR.DTO.Inbound;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IAbsenceRepo
    {
        Task<Absence> AddOrRequestAbsence(AddAbsenceRequest absenceRequest, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int myId, int companyId, bool IsApproved);
    }
}
