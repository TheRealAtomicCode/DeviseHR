using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.outbound;
using HR.Repository;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.Subroutines;
using HR.UOW.Interfaces;
using Mapster;
using Models;
using System.ComponentModel.Design;


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
        public async Task<CreateContractDto> CalculateLeaveYear(CreateContractDto newContract, int companyId)
        {
            if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (newContract.ContractType == 3) throw new Exception("Developer Error: Working Patterns need to be added first");

            if (newContract.TermTimeId != 0) throw new Exception("Developer Error: Term times need to be added first");

            var employee = await _mainUOW.EmployeeRepo.GetEmployeeById(newContract.EmployeeId, companyId);
            if (employee == null) throw new Exception("Employee not found");

            DateOnly annualLeaveStartDate = DateModifier.GetLeaveYearStartDate(newContract.ContractStartDate, employee.AnnualLeaveStartDate);

            List<Contract> existingContracts = await _mainUOW.ContractRepo.GetContractsByLeaveYear(employee.Id, employee.CompanyId, annualLeaveStartDate);

            int leaveUnit = ContractSubroutines.CheckAndGetLeaveUnit(existingContracts, newContract);

            int previousLeaveYearEntitlement = CalculateContract.CalculateLeaveYearEntitlement(existingContracts, annualLeaveStartDate, newContract.ContractStartDate, leaveUnit);

            NewContractCalculationResult newContractResult = CalculateContract.CalculateNewContractEntitlementMut(newContract, annualLeaveStartDate, leaveUnit);

            // double thisLeaveYearsAllowance = previousLeaveYearEntitlement + newContractResult.ThisContractEntitlement;
            double thisLeaveYearsAllowance = newContractResult.ThisContractEntitlement;

            newContract.FirstLeaveAllowence = NumberUtils.RoundCustom(thisLeaveYearsAllowance + previousLeaveYearEntitlement);
            newContract.NextLeaveAllowence = NumberUtils.RoundCustom(newContractResult.ContractleaveEntitlementPerYear);
            newContract.ContractedLeaveEntitlement = NumberUtils.RoundCustom(newContractResult.ContractleaveEntitlementPerYear);

            return newContract;
        }





        public async Task<ContractDto> CreateContract(CreateContractDto newContract, int myId, int companyId, int userRole)
        {
            if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (newContract.ContractType == 3) throw new Exception("Developer Error: Working Patterns need to be added first");

            if (newContract.TermTimeId != 0) throw new Exception("Developer Error: Term times need to be added first");

            //  Verifying who can add contracts to who
            if (userRole >= StaticRoles.Admin)
            {
                if (newContract.EmployeeId == myId)
                {
                    bool hasManager = await _mainUOW.HierarchyRepo.HasManager(myId);
                    if (hasManager) throw new Exception("Admins with a manager asigned to their profile can not add contracts to their own accounts.");
                }
            }

            if (userRole == StaticRoles.Manager)
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

            Contract addedContract = await _mainUOW.ContractRepo.AddContract(employee, newContract, myId, companyId);

            await _mainUOW.SaveChangesAsync();

            ContractDto addedContractDto = addedContract.Adapt<ContractDto>();

            return addedContractDto;
        }



        public async Task<ContractAndLeaveYearCount> GetLeaveYear(DateOnly reuestedDate, int employeeId, int myId, int userType, int companyId)
        {
            int requestAddOrError = await ValidateRequestOrAddAbsence(myId, userType, employeeId);

            // 1 add
            // 0 request
            // -1 add
            if (requestAddOrError == 1 || requestAddOrError == 0)
            {
                var employee = await _mainUOW.EmployeeRepo.GetEmployeeById(employeeId, companyId);

                if (employee == null) throw new Exception("Employee not found");

                var requestedAnnualeLeaveYearStartDate = DateModifier.GetLeaveYearStartDate(reuestedDate, employee.AnnualLeaveStartDate);

                //
                //
                //
                // NOTE
                // DELETE WHEN WORKING ON FRONT END IF NEEDED
                if (requestedAnnualeLeaveYearStartDate != reuestedDate) throw new Exception("Must provide an annual leave year start date");

                Contract? firstContract = await _mainUOW.ContractRepo.GetFirstContractOrDefault(employee.Id, employee.CompanyId);

                if (firstContract == null) throw new Exception("Employee does not have any contracts");

                Contract? lastContract = await _mainUOW.ContractRepo.GetLastContractByDate(employee.Id, employee.CompanyId, requestedAnnualeLeaveYearStartDate.AddYears(1).AddDays(-1));

                ContractDto lastContractDto = lastContract.Adapt<ContractDto>();

                if (lastContract == null) throw new Exception("Employee does not have any contracts");

                List<StartAndEndDate> leaveYears = ContractSubroutines.GetLeaveYearCount(reuestedDate, employee.AnnualLeaveStartDate, firstContract.ContractStartDate);

                ContractAndLeaveYearCount contractAndLeaveYears = new ContractAndLeaveYearCount{
                    contract = lastContractDto,
                    leaveYears = leaveYears
                };

                return contractAndLeaveYears;
            }

            throw new Exception("You do not have sufficiant permissions to view this profile.");

        }



        

    }
}
