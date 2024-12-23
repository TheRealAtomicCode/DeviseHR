using HR.DTO.Inbound;
using HR.Repository.Interfaces;
using Models;

namespace HR.Repository
{
    public class AbsenceRepo : IAbsenceRepo
    {

        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public AbsenceRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }


        public async Task<Absence> AddOrRequestAbsence(AddAbsenceRequest absenceRequest, DateOnly startDate, DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int myId, int companyId, bool IsApproved)
        {
            int approvedBy = 0;
            if (IsApproved == true) approvedBy = myId;

            Absence absence = new Absence
            {
                AbsenceStartDate = startDate,
                AbsenceEndDate = endDate,
                StartTime = startTime,
                EndTime = endTime,
                IsApproved = IsApproved,
                IsPending = !IsApproved,
                ApprovedId = approvedBy,
                ApprovedByAdmin = 0,
                AbsenceTypeId = absenceRequest.AbsenceType,
                DaysDeducted = absenceRequest.TimeDeducted,
                HoursDeducted = absenceRequest.TimeDeducted,
                AddedBy = myId,
            };

            await _context.Absences.AddAsync(absence);

            return absence;
        }


    }
}
