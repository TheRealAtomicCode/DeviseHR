using HR.DTO.Inbound;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IAbsenceRepo
    {
        Task<Absence> AddAbsence(Absence absence);
    }
}
