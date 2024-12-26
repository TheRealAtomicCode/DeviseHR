using Models;

namespace HR.Repository.Interfaces
{
    public interface IAbsenceRepo
    {
        Task<Absence> AddAbsence(Absence absence);
        Task<List<Absence>> GetAbsencesLocatedBetweenDates(DateOnly absenceStartDate, DateOnly absenceEndDate, int employeeId, int contractId);
        Task<Absence> GetAbsenceLocatedInDateOrDefault(DateOnly selectedDate, int employeeId, int companyId);
    }
}
