using HR.DTO.Inbound;
using HR.DTO;
using HR.Services.Interfaces;
using HR.UOW.Interfaces;
using HR.Repository;
using Models;
using System;
using Mapster;

namespace HR.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IMainUOW _mainUOW;
        private readonly IConfiguration _configuration;

        public AbsenceService(IMainUOW mainUOW, IConfiguration configuration)
        {
            _mainUOW = mainUOW;
            _configuration = configuration;
        }

        public async Task<AbsenceDto> AddOrRequestAbsence(AddAbsenceRequest absenceRequest, int myId, int companyId, int myRole)
        {
            DateOnly startDate = DateOnly.FromDateTime(absenceRequest.StartDateTime);
            DateOnly endDate = DateOnly.FromDateTime(absenceRequest.EndDateTime);

            TimeOnly startTime = TimeOnly.FromDateTime(absenceRequest.StartDateTime);
            TimeOnly endTime = TimeOnly.FromDateTime(absenceRequest.EndDateTime);

            //throw new Exception("Check if ContractRepo.GetContractsThatFallBetween() is in use by another method, if not then change it, else create a new method to get contracts to add the absence.");
            List<Contract> contracts = await _mainUOW.ContractRepo.GetContractsBetweenDates(startDate, endDate, absenceRequest.EmployeeId, companyId);

            if (contracts.Count == 0) throw new Exception("Employee has no contracts");
            if (contracts.Count > 1) throw new Exception("Can not add absence between 2 contracts");

            int requestAddOrError = await _mainUOW.HierarchyRepo.ValidateRequestOrAddAbsence(myId, myRole, absenceRequest.EmployeeId);

            int absenceState = 0;
            if (requestAddOrError == 1) absenceState = 2;
            if (requestAddOrError == -1) throw new Exception("Your permissions are not sufficiant for the operation");

            int? approvedBy = null;
            if(absenceState == 1) approvedBy = myId;

            Absence absence = new Absence
            {
                AbsenceStartDate = startDate,
                AbsenceEndDate = endDate,
                StartTime = startTime,
                EndTime = endTime,
                AbsenceState = absenceState,
                ApprovedBy = approvedBy,
                ApprovedByAdmin = 0,
                AbsenceTypeId = absenceRequest.AbsenceType,
                DaysDeducted = absenceRequest.TimeDeducted,
                HoursDeducted = absenceRequest.TimeDeducted,
                AddedBy = myId,
            };

            var absenceDto = absence.Adapt<AbsenceDto>();

            return absenceDto;
        }
    }
}
