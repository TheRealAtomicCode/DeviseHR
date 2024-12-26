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
            if (absenceRequest.AbsenceEndDate < absenceRequest.AbsenceStartDate) throw new Exception("Absences end dates must be after the absence start date");
            if (absenceRequest.AbsenceEndDate == absenceRequest.AbsenceStartDate && absenceRequest.EndTime <= absenceRequest.StartTime) throw new Exception("Absences end time must be after the absence start time");

            //throw new Exception("Check if ContractRepo.GetContractsThatFallBetween() is in use by another method, if not then change it, else create a new method to get contracts to add the absence.");
            List<Contract> contracts = await _mainUOW.ContractRepo.GetContractsThatFallInDates(absenceRequest.EmployeeId, companyId, absenceRequest.AbsenceStartDate, absenceRequest.AbsenceEndDate);

            if (contracts.Count > 1) throw new Exception("Can not add absence between 2 contracts");

            var existingContract = contracts.FirstOrDefault();
            if (existingContract == null) throw new Exception("Employee has no contracts in provided date");

            int employeeId = contracts[0].EmployeeId;
            int contractId = contracts[0].Id;

            if (existingContract.ContractStartDate > absenceRequest.AbsenceStartDate) throw new Exception("Can not add absence before the start of the contract");

            if (absenceRequest.IsDays != existingContract.IsDays) throw new Exception("The leave unit for the employee's existng contract does not match the requested leave unit");
            if (existingContract.IsDays == false && absenceRequest.IsFirstHalfDay != null) throw new Exception("You can not add half days to contracts that are in hours");

            // get absences that fall between the absence request dates if any exist, error
            var existingAbsences = await _mainUOW.AbsenceRepo.GetAbsencesLocatedBetweenDates(absenceRequest.AbsenceStartDate, absenceRequest.AbsenceEndDate, employeeId, contractId);
            if (existingAbsences.Count > 0) throw new Exception("Can not add absences over existing absences, please edit the existing absence, or try deleting and addinng the absence again");

            int requestAddOrError = await _mainUOW.HierarchyRepo.ValidateRequestOrAddAbsence(myId, myRole, existingContract.EmployeeId);

            int absenceState = 0;
            if (requestAddOrError == 1) absenceState = 2;
            if (requestAddOrError == -1) throw new Exception("Your permissions are not sufficiant for the operation");

            int? approvedBy = null;
            if(absenceState == 1) approvedBy = myId;

            var absence = absenceRequest.Adapt<Absence>();
            absence.AddedBy = myId;
            absence.CompanyId = companyId;
            absence.ContractId = existingContract.Id;
            absence.ApprovedBy = approvedBy;
            absence.AbsenceState = absenceState;

            await _mainUOW.AbsenceRepo.AddAbsence(absence);

            await _mainUOW.SaveChangesAsync();

            var absenceDto = absence.Adapt<AbsenceDto>();

            return absenceDto;
        }
    }
}
