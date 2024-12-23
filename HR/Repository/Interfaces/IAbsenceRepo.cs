using HR.DTO.Inbound;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IAbsenceRepo
    {
        Task<Absence> AddOrRequestAbsence(Absence absence);
    }
}
