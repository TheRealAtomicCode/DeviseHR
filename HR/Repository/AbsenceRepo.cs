using Microsoft.EntityFrameworkCore;
using Models;

namespace HR.Repository
{
    public class AbsenceRepo
    {

        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public AbsenceRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }


        public async Task<Absence> AddAbsence(Absence absence)
        {
            await _context.Absences.AddAsync(absence);

            return absence;
        }


        public async Task<List<Absence>> GetAbsencesLocatedBetweenDates(DateOnly absenceStartDate, DateOnly absenceEndDate, int employeeId, int contractId)
        {
            var absences = await _context.Absences
                .Where(a => a.EmployeeId == employeeId &&
                            a.ContractId == contractId &&
                            (
                                (a.AbsenceStartDate >= absenceStartDate && a.AbsenceStartDate <= absenceEndDate) ||
                                (a.AbsenceEndDate >= absenceStartDate && a.AbsenceEndDate <= absenceEndDate) ||
                                (a.AbsenceStartDate <= absenceStartDate && a.AbsenceEndDate >= absenceEndDate)
                            ))
                .ToListAsync();

            return absences;
        }

        public async Task<Absence> GetAbsenceLocatedInDateOrDefault(DateOnly selectedDate, int employeeId, int contractId)
        {
            var absence = await _context.Absences
                .Where(a => a.EmployeeId == employeeId &&
                            a.ContractId == contractId &&
                            (
                                a.AbsenceStartDate <= selectedDate && a.AbsenceEndDate >= selectedDate
                            ))
                .FirstOrDefaultAsync();

            return absence;
        }



    }
}
