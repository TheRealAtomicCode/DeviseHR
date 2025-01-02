using Common;
using HR.DTO;
using HR.Repository;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.Subroutines;
using HR.UOW.Interfaces;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;



namespace HR.Services
{
    public class ContractService : IContractService
    {

        private readonly IMainUOW _mainUOW;
        private readonly IConfiguration _configuration;

        public ContractService(IMainUOW mainUOW, IConfiguration configuration)
        {
            _mainUOW = mainUOW;
            _configuration = configuration;
        }

        //public async Task<CreateContractDto> CalculateLeaveYear(CreateContractDto newContract, int companyId)
        //{
        //    if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

        //    if (newContract.ContractType == 3) throw new Exception("Developer Error: Working Patterns need to be added first");

        //    if (newContract.TermTimeId != 0) throw new Exception("Developer Error: Term times need to be added first");

        //    var employee = await _contractRepo.GetEmployeeById(newContract.employeeId, companyId);

        //    if (employee == null) throw new Exception("Employee not found");

        //    DateOnly annualLeaveStartDate = DateModifier.GetLeaveYearStartDate(newContract.StartDate, employee.AnnualLeaveStartDate);

        //    List<Contract> existingContracts = await _contractRepo.GetContractByLeaveYear(employee, annualLeaveStartDate);

        //    bool isDays = ContractVerification.CheckAndGetLeaveUnit(existingContracts, newContract);

        //    int previousLeaveYearEntitlement = CalculateContract.CalculateLeaveYearEntitlement(existingContracts, annualLeaveStartDate, newContract.StartDate, isDays);

        //    NewContractCalculationResult newContractResult = CalculateContract.CalculateNewContractEntitlementMut(newContract, newContract.StartDate, annualLeaveStartDate, isDays);

        //    double thisLeaveYearsAllowance = previousLeaveYearEntitlement + newContractResult.ThisContractEntitlement;

        //    newContract.FirstLeaveAllowence = (int)Math.Ceiling(thisLeaveYearsAllowance);
        //    newContract.NextLeaveAllowence = (int)Math.Ceiling(newContractResult.ContractleaveEntitlementPerYear);
        //    newContract.ContractedLeaveEntitlement = (int)Math.Ceiling(newContractResult.ContractleaveEntitlementPerYear);

        //    return newContract;
        //}


        // calculating anual leave for days variable 2
        public async Task<AddContractRequest> CalculateLeaveYear(AddContractRequest newContract, int companyId)
        {
            if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (newContract.ContractType == 3)
            {
                // fixed contracts
                if (newContract.PatternId == null) throw new Exception("Fixed contracts must have an asigned working pattern");

                var workingPattern = await _mainUOW.WorkingPatternRepo.GetWorkingPatternByIdOrDefault((int)newContract.PatternId, companyId);

                if (workingPattern == null) throw new Exception("Working pattern not found");

                (int workingDays, int workingHours) = WorkingPatternSubroutines.ExtractWorkingDaysAndHours(workingPattern);

                newContract.AverageWorkingDay = workingDays;
                newContract.ContractedDaysPerWeek = workingDays;
                newContract.ContractedHoursPerWeek = workingHours;
            }

            if (newContract.TermTimeId != 0) throw new Exception("Developer Error: Term times need to be added first");

            var employee = await _mainUOW.EmployeeRepo.GetEmployeeById(newContract.EmployeeId, companyId);
            if (employee == null) throw new Exception("Employee not found");

            DateOnly annualLeaveStartDate = DateModifier.GetLeaveYearStartDate(newContract.ContractStartDate, employee.AnnualLeaveStartDate);
            DateOnly annualLeaveEndDate = annualLeaveStartDate.AddYears(1).AddDays(-1);

            List<Contract> existingContracts = new List<Contract>();

            if (newContract.ContractStartDate > annualLeaveStartDate)
            {
                existingContracts = await _mainUOW.ContractRepo.GetContractsThatFallBetweenDates(employee.Id, employee.CompanyId, annualLeaveStartDate, annualLeaveEndDate);

                // validate when the contracts were added in order to calculate
                // this has been added to calculate contracts when editing a contract
                if (newContract.CalculateForEdit == true)
                {
                    if (existingContracts[existingContracts.Count - 1].ContractStartDate == newContract.ContractStartDate || existingContracts.Count == 1)
                    {
                        existingContracts.RemoveAt(existingContracts.Count - 1);
                    }
                    else if (existingContracts[existingContracts.Count - 1].ContractStartDate > newContract.ContractStartDate && existingContracts.Count > 1)
                    {
                        if (existingContracts[existingContracts.Count - 2].ContractStartDate < newContract.ContractStartDate)
                        {
                            existingContracts.RemoveAt(existingContracts.Count - 1);
                        }
                        else
                        {
                            throw new Exception("A contract already exists before the contract you are editing that starts after your selected date");
                        }
                    }

             
                }



            }

            if (existingContracts.Count > 0)
            {
                if (newContract.ContractStartDate < existingContracts[existingContracts.Count - 1].ContractStartDate) throw new Exception("A contract already exists before the new contracts date");
            }

            var virtualContracts = ContractSubroutines.VirtualizeContracts(existingContracts);

            int leaveUnit = ContractSubroutines.CheckAndGetLeaveUnit(virtualContracts, newContract);

            var previousLeaveYearContracts = Calculate.PlaceContractsInYear(virtualContracts, annualLeaveStartDate);

            // set last contract to end at contrcat start date
            if (previousLeaveYearContracts.Count > 0)
            {
                previousLeaveYearContracts[previousLeaveYearContracts.Count - 1].ContractEndDate = newContract.ContractStartDate.AddDays(-1);
            }

            int previousLeaveYearEntitlement = Calculate.CalculateLeaveYearEntitlementByDates(previousLeaveYearContracts, annualLeaveStartDate, newContract.ContractStartDate, leaveUnit);

            NewContractCalculationResult newContractResult = Calculate.CalculateNewContractEntitlementMut(newContract, annualLeaveStartDate, leaveUnit);

            // double thisLeaveYearsAllowance = previousLeaveYearEntitlement + newContractResult.ThisContractEntitlement;
            double thisLeaveYearsAllowance = newContractResult.ThisContractEntitlement;

            newContract.FirstLeaveAllowence = NumberUtils.RoundCustom(thisLeaveYearsAllowance + previousLeaveYearEntitlement);
            newContract.NextLeaveAllowence = NumberUtils.RoundCustom(newContractResult.ContractleaveEntitlementPerYear);
            newContract.ContractedLeaveEntitlement = NumberUtils.RoundCustom(newContractResult.ContractleaveEntitlementPerYear);

            return newContract;
        }





        public async Task<ContractDto> CreateContract(AddContractRequest newContract, int myId, int companyId, int myRole)
        {
            if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (newContract.ContractType == 3)
            {
                // fixed contracts
                if (newContract.PatternId == null) throw new Exception("Fixed contracts must have an asigned working pattern");

                var workingPattern = await _mainUOW.WorkingPatternRepo.GetWorkingPatternByIdOrDefault((int)newContract.PatternId, companyId);

                if (workingPattern == null) throw new Exception("Working pattern not found");

                (int workingDays, int workingHours) = WorkingPatternSubroutines.ExtractWorkingDaysAndHours(workingPattern);

                newContract.AverageWorkingDay = workingDays;
                newContract.ContractedDaysPerWeek = workingDays;
                newContract.ContractedHoursPerWeek = workingHours;
            }

            if (newContract.TermTimeId != 0) throw new Exception("Developer Error: Term times need to be added first");

            //  Verifying who can add contracts to who
            if (myRole >= StaticRoles.Admin)
            {
                if (newContract.EmployeeId == myId)
                {
                    bool hasManager = await _mainUOW.HierarchyRepo.HasManager(myId);
                    if (hasManager) throw new Exception("Admins with a manager asigned to their profile can not add contracts to their own accounts.");
                }
            }

            if (myRole == StaticRoles.Manager)
            {
                if (newContract.EmployeeId == myId) throw new Exception("Managers can not add conracts to their own profiles, please contact your Admin.");

                bool isSubordinate = await _mainUOW.HierarchyRepo.IsRelated(myId, newContract.EmployeeId);

                if (!isSubordinate) throw new Exception("You can not add contracts to users who are not your direct subordinates.");
            }

            var employee = await _mainUOW.EmployeeRepo.GetEmployeeById(newContract.EmployeeId, companyId);
            if (employee == null) throw new Exception("Employee not found");

            Contract? lastContract = await _mainUOW.ContractRepo.GetLastContractOrDefault(employee.Id, employee.CompanyId);

            if (lastContract != null && lastContract.IsDays != newContract.IsDays) throw new Exception("Can not add contracts with different leave units.");

            if (lastContract != null && lastContract.ContractStartDate >= newContract.ContractStartDate) throw new Exception("Can not add contact before previous contract start date.");

            var absence = await _mainUOW.AbsenceRepo.GetAbsenceLocatedInDateOrDefault(newContract.ContractStartDate, employee.Id, employee.CompanyId);

            if (absence != null) throw new Exception("Cannot add a new contract during an active leave period; please adjust the contract start date or re-add absences accordingly.");

            Contract addedContract = await _mainUOW.ContractRepo.AddContract(employee, newContract, myId, companyId);

            await _mainUOW.SaveChangesAsync();

            ContractDto addedContractDto = addedContract.Adapt<ContractDto>();

            return addedContractDto;
        }




        //public async Task EditLastContract(JsonPatchDocument<EditContractRequest> patchDoc, int employeeId, int myId, int myRole, int companyId)
        //{
        //    var editedContract = patchDoc.Adapt<EditContractRequest>();

        //    if (editedContract.ContractType == 3)
        //    {
        //        // fixed contracts
        //        if (editedContract.PatternId == null) throw new Exception("Fixed contracts must have an asigned working pattern");

        //        var workingPattern = await _mainUOW.WorkingPatternRepo.GetWorkingPatternByIdOrDefault((int)editedContract.PatternId, companyId);

        //        if (workingPattern == null) throw new Exception("Working pattern not found");

        //        (int workingDays, int workingHours) = WorkingPatternSubroutines.ExtractWorkingDaysAndHours(workingPattern);

        //        editedContract.AverageWorkingDay = workingDays;
        //        editedContract.ContractedDaysPerWeek = workingDays;
        //        editedContract.ContractedHoursPerWeek = workingHours;
        //    }

        //    if (editedContract.TermTimeId != 0) throw new Exception("Developer Error: Term times need to be added first");

        //    //  Verifying who can add contracts to who
        //    if (myRole >= StaticRoles.Admin)
        //    {
        //        if (employeeId == myId)
        //        {
        //            bool hasManager = await _mainUOW.HierarchyRepo.HasManager(myId);
        //            if (hasManager) throw new Exception("Admins with a manager asigned to their profile can not add contracts to their own accounts.");
        //        }
        //    }

        //    if (myRole == StaticRoles.Manager)
        //    {
        //        if (employeeId == myId) throw new Exception("Managers can not add conracts to their own profiles, please contact your Admin.");

        //        bool isSubordinate = await _mainUOW.HierarchyRepo.IsRelated(myId, employeeId);

        //        if (!isSubordinate) throw new Exception("You can not add contracts to users who are not your direct subordinates.");
        //    }

        //    var employee = await _mainUOW.EmployeeRepo.GetEmployeeById(employeeId, companyId);
        //    if (employee == null) throw new Exception("Employee not found");

        //    var last2Contracts = await _mainUOW.ContractRepo.GetLast2Contracts(employee.Id, employee.CompanyId);

        //    Contract? lastContract = null;
        //    Contract? contractBeforeLast = null;

        //    if (last2Contracts.Count >= 2) contractBeforeLast = last2Contracts[1];
        //    if (last2Contracts.Count >= 1) lastContract = last2Contracts[0];
        //    if (lastContract == null) throw new Exception("Contract not found");

        //    if (contractBeforeLast != null && contractBeforeLast.ContractStartDate >= editedContract.ContractStartDate) throw new Exception("Can not edit contact before previous contract start date.");

        //    var absence = await _mainUOW.AbsenceRepo.GetAbsenceLocatedInDateOrDefault(editedContract.ContractStartDate, employee.Id, employee.CompanyId);

        //    if (absence != null) throw new Exception("Cannot add a new contract during an active leave period; please adjust the contract start date or re-add absences accordingly.");

        //    var toPatch = lastContract.Adapt<EditContractRequest>();

        //    patchDoc.ApplyTo(toPatch);
        //    toPatch.Adapt(lastContract);

        //    await _mainUOW.SaveChangesAsync();
        //}

        public async Task EditLastContract(EditContractRequest editContractRequest, int employeeId, int myId, int myRole, int companyId)
        {
            var employee = await _mainUOW.EmployeeRepo.GetEmployeeById(employeeId, companyId);
            if (employee == null) throw new Exception("Employee not found");

            var last2Contracts = await _mainUOW.ContractRepo.GetLast2Contracts(employee.Id, employee.CompanyId);

            Contract? lastContract = null;
            Contract? contractBeforeLast = null;

            if (last2Contracts.Count >= 2) contractBeforeLast = last2Contracts[1];
            if (last2Contracts.Count >= 1) lastContract = last2Contracts[0];
            if (lastContract == null) throw new Exception("Contract not found");

            if(editContractRequest.ContractStartDate != null)
            {
                if (contractBeforeLast != null && contractBeforeLast.ContractStartDate >= editContractRequest.ContractStartDate) throw new Exception("Can not edit contact before previous contract start date.");
            }

            if (editContractRequest.ContractType == 3)
            {
                // fixed contracts
                if (editContractRequest.PatternId == null) throw new Exception("Fixed contracts must have an asigned working pattern");

                var workingPattern = await _mainUOW.WorkingPatternRepo.GetWorkingPatternByIdOrDefault((int)editContractRequest.PatternId, companyId);

                if (workingPattern == null) throw new Exception("Working pattern not found");

                (int workingDays, int workingHours) = WorkingPatternSubroutines.ExtractWorkingDaysAndHours(workingPattern);

                editContractRequest.AverageWorkingDay = workingDays;
                editContractRequest.ContractedDaysPerWeek = workingDays;
                editContractRequest.ContractedHoursPerWeek = workingHours;
            }

            if (editContractRequest.TermTimeId != null) throw new Exception("Developer Error: Term times need to be added first");

            //  Verifying who can add contracts to who
            if (myRole >= StaticRoles.Admin)
            {
                if (employeeId == myId)
                {
                    bool hasManager = await _mainUOW.HierarchyRepo.HasManager(myId);
                    if (hasManager) throw new Exception("Admins with a manager asigned to their profile can not add contracts to their own accounts.");
                }
            }

            if (myRole == StaticRoles.Manager)
            {
                if (employeeId == myId) throw new Exception("Managers can not add conracts to their own profiles, please contact your Admin.");

                bool isSubordinate = await _mainUOW.HierarchyRepo.IsRelated(myId, employeeId);

                if (!isSubordinate) throw new Exception("You can not add contracts to users who are not your direct subordinates.");
            }

            // edit contract
            lastContract.PatternId = editContractRequest.PatternId.HasValue ? editContractRequest.PatternId : lastContract.PatternId;
            lastContract.ContractType = editContractRequest.ContractType.HasValue ? (int)editContractRequest.ContractType : lastContract.ContractType;
            lastContract.ContractStartDate = editContractRequest.ContractStartDate.HasValue ? (DateOnly)editContractRequest.ContractStartDate : lastContract.ContractStartDate;
            lastContract.ContractedHoursPerWeek = editContractRequest.ContractedHoursPerWeek.HasValue ? (int)editContractRequest.ContractedHoursPerWeek : lastContract.ContractedHoursPerWeek;
            lastContract.ContractedDaysPerWeek = editContractRequest.ContractedDaysPerWeek.HasValue ? (int)editContractRequest.ContractedDaysPerWeek : lastContract.ContractedDaysPerWeek;
            lastContract.CompanyHoursPerWeek = editContractRequest.CompanyHoursPerWeek.HasValue ? (int)editContractRequest.CompanyHoursPerWeek : lastContract.CompanyHoursPerWeek;
            lastContract.CompanyDaysPerWeek = editContractRequest.CompanyDaysPerWeek.HasValue ? (int)editContractRequest.CompanyDaysPerWeek : lastContract.CompanyDaysPerWeek;
            lastContract.AverageWorkingDay = editContractRequest.AverageWorkingDay.HasValue ? (int)editContractRequest.AverageWorkingDay : lastContract.AverageWorkingDay;
            lastContract.CompanyLeaveEntitlement = editContractRequest.CompanyLeaveEntitlement.HasValue ? (int)editContractRequest.CompanyLeaveEntitlement : lastContract.CompanyLeaveEntitlement;
            lastContract.ContractedLeaveEntitlement = editContractRequest.ContractedLeaveEntitlement.HasValue ? (int)editContractRequest.ContractedLeaveEntitlement : lastContract.ContractedLeaveEntitlement;
            lastContract.FirstLeaveAllowence = editContractRequest.FirstLeaveAllowence.HasValue ? (int)editContractRequest.FirstLeaveAllowence : lastContract.FirstLeaveAllowence;
            lastContract.NextLeaveAllowence = editContractRequest.NextLeaveAllowence.HasValue ? (int)editContractRequest.NextLeaveAllowence : lastContract.NextLeaveAllowence;
            lastContract.TermTimeId = editContractRequest.TermTimeId.HasValue ? editContractRequest.TermTimeId : lastContract.TermTimeId;
            lastContract.DiscardedId = editContractRequest.DiscardedId.HasValue ? editContractRequest.DiscardedId : lastContract.DiscardedId;


            var absence = await _mainUOW.AbsenceRepo.GetAbsenceLocatedInDateOrDefault(lastContract.ContractStartDate, employee.Id, employee.CompanyId);

            if (absence != null) throw new Exception("Cannot add a new contract during an active leave period; please adjust the contract start date or re-add absences accordingly.");

            
            // update absences



             await _mainUOW.SaveChangesAsync();
        }




        public async Task<LeaveYearResponse> GetLeaveYear(DateOnly reuestedDate, int employeeId, int myId, int myRole, int companyId)
        {
            int requestAddOrError = await _mainUOW.HierarchyRepo.ValidateRequestOrAddAbsence(myId, myRole, employeeId);

            // 1 add
            // 0 request
            // -1 add
            if (requestAddOrError == 1 || requestAddOrError == 0)
            {
                var employee = await _mainUOW.EmployeeRepo.GetEmployeeById(employeeId, companyId);

                if (employee == null) throw new Exception("Employee not found");

                var annualLeaveStartDate = DateModifier.GetLeaveYearStartDate(reuestedDate, employee.AnnualLeaveStartDate);
                var annualLeaveEndDate = annualLeaveStartDate.AddYears(1).AddDays(-1);
                //
                //
                //
                // NOTE
                // DELETE WHEN WORKING ON FRONT END IF NEEDED
                if (annualLeaveStartDate != reuestedDate) throw new Exception("Must provide an annual leave year start date");

                Contract? firstContract = await _mainUOW.ContractRepo.GetFirstContractOrDefault(employee.Id, employee.CompanyId);

                if (firstContract == null) throw new Exception("Employee does not have any contracts");

                // Contract? lastContract = await _mainUOW.ContractRepo.GetLastContractByDateOrDefault(employee.Id, employee.CompanyId, annualLeaveEndDate);

                // ContractDto lastContractDto = lastContract.Adapt<ContractDto>();

                if (firstContract == null) throw new Exception("Employee does not have any contracts");

                List<StartAndEndDate> leaveYears = ContractSubroutines.GetLeaveYearCount(reuestedDate, employee.AnnualLeaveStartDate, firstContract.ContractStartDate);

                List<Contract> existingContracts = await _mainUOW.ContractRepo.GetContractsThatFallBetweenDates(employee.Id, employee.CompanyId, annualLeaveStartDate, annualLeaveEndDate);

                var virtualContracts = ContractSubroutines.VirtualizeContracts(existingContracts);

                var leaveYearConracts = Calculate.PlaceContractsInYear(virtualContracts, annualLeaveStartDate);

                var absences = await _mainUOW.AbsenceRepo.GetAbsencesLocatedBetweenDates(annualLeaveStartDate, annualLeaveEndDate, employee.Id, employee.CompanyId);

                var absenceDtos = absences.Adapt<List<AbsenceDto>>();

                LeaveYearResponse contractAndLeaveYears = new LeaveYearResponse
                {
                    leaveYearContracts = leaveYearConracts,
                    absences = absenceDtos,
                    leaveYears = leaveYears
                };

                return contractAndLeaveYears;
            }

            throw new Exception("You do not have sufficiant permissions to view this profile.");

        }


        public async Task<ContractDto> DetatchWorkingPattern(int contractId, int myId, int myRole, int companyId)
        {
            var contract = await _mainUOW.ContractRepo.GetContractByIdOrDefault(contractId, companyId);

            if (contract == null) throw new Exception("Contract not found");

            //  Verifying who can add contracts to who
            if (myRole >= StaticRoles.Admin)
            {
                if (contract.EmployeeId == myId)
                {
                    bool hasManager = await _mainUOW.HierarchyRepo.HasManager(myId);
                    if (hasManager) throw new Exception("Admins with a manager asigned to their profile can not edit their own contracts.");
                }
            }

            if (myRole == StaticRoles.Manager)
            {
                if (contract.EmployeeId == myId) throw new Exception("Managers can not Edit their own contracts.");

                bool isSubordinate = await _mainUOW.HierarchyRepo.IsRelated(myId, contract.EmployeeId);

                if (!isSubordinate) throw new Exception("You can not edit contracts to users who are not your direct subordinates.");
            }


            if (contract.PatternId == 0 || contract.PatternId == null) throw new Exception("Contract has no asigned Working pattern");

            contract.ContractType = 2;
            contract.PatternId = null;

            await _mainUOW.SaveChangesAsync();

            var contractDto = contract.Adapt<ContractDto>();

            return contractDto;
        }

  
    }
}
