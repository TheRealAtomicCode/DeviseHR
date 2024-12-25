using HR.DTO.Inbound;
using Models;
using System.Diagnostics.Contracts;

namespace HR.Repository.Interfaces
{
    public interface IAbsenceRepo
    {
        Task<Absence> AddAbsence(Absence absence);
        Task<List<Absence>> GetAbsencesLocatedBetweenDates(DateOnly absenceStartDate, DateOnly absenceEndDate, int employeeId, int contractId);
    }
}
