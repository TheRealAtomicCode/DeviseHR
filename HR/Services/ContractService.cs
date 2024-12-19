using Common;
using HR.DTO;
using HR.DTO.Inbound;
using HR.Repository;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.Subroutines;
using Models;

namespace HR.Services
{
    public class ContractService : IContractService
    {

        private readonly IContractRepo _contractRepo;
        private readonly IConfiguration _configuration;

        public ContractService(IContractRepo contractRepo, IConfiguration configuration)
        {
            _contractRepo = contractRepo;
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

            var employee = await _contractRepo.GetEmployeeById(newContract.EmployeeId, companyId);
            if (employee == null) throw new Exception("Employee not found");

            DateOnly annualLeaveStartDate = DateModifier.GetLeaveYearStartDate(newContract.ContractStartDate, employee.AnnualLeaveStartDate);

            List<Contract> existingContracts = await _contractRepo.GetContractByLeaveYear(employee, annualLeaveStartDate);

            int leaveUnit = ContractVerification.CheckAndGetLeaveUnit(existingContracts, newContract);

            int previousLeaveYearEntitlement = CalculateContract.CalculateLeaveYearEntitlement(existingContracts, annualLeaveStartDate, newContract.ContractStartDate, leaveUnit);

            NewContractCalculationResult newContractResult = CalculateContract.CalculateNewContractEntitlementMut(newContract, annualLeaveStartDate, leaveUnit);

            // double thisLeaveYearsAllowance = previousLeaveYearEntitlement + newContractResult.ThisContractEntitlement;
            double thisLeaveYearsAllowance = newContractResult.ThisContractEntitlement;

            newContract.FirstLeaveAllowence = (int)Math.Ceiling(thisLeaveYearsAllowance + previousLeaveYearEntitlement);
            newContract.NextLeaveAllowence = (int)Math.Ceiling(newContractResult.ContractleaveEntitlementPerYear);
            newContract.ContractedLeaveEntitlement = (int)Math.Ceiling(newContractResult.ContractleaveEntitlementPerYear);

            return newContract;
        }







        public async Task<Contract> CreateContract(CreateContractDto newContract, int myId, int companyId, int userRole)
        {
            if (newContract.ContractType == 1) throw new Exception("Contract does not require calculation");

            if (newContract.ContractType == 3) throw new Exception("Developer Error: Working Patterns need to be added first");

            if (newContract.TermTimeId != 0) throw new Exception("Developer Error: Term times need to be added first");

            //  Verifying who can add contracts to who
            if (userRole >= StaticRoles.Admin)
            {
                if (newContract.EmployeeId == myId)
                {
                    bool hasManager = await _contractRepo.HasManager(myId);
                    if (hasManager) throw new Exception("Admins with a manager asigned to their profile can not add contracts to their own accounts.");
                }
            }

            if (userRole == StaticRoles.Manager)
            {
                if (newContract.EmployeeId == myId) throw new Exception("Managers can not add conracts to their own profiles, please contact your Admin.");

                bool isSubordinate = await _contractRepo.IsRelated(myId, newContract.EmployeeId);

                if (!isSubordinate) throw new Exception("You can not add contracts to users who are not your direct subordinates.");
            }

            var employee = await _contractRepo.GetEmployeeById(newContract.EmployeeId, companyId);
            if (employee == null) throw new Exception("Employee not found");

            Contract? lastContract = await _contractRepo.GetLastContractOrDefault(employee);

            if (lastContract != null && lastContract.IsDays != newContract.IsDays) throw new Exception("Can not add contracts with different leave units.");

            if (lastContract != null && lastContract.ContractStartDate >= newContract.ContractStartDate) throw new Exception("Can not add contact before previous contract start date.");

            Contract addedContract = await _contractRepo.AddContract(employee, newContract, myId, companyId);

            await _contractRepo.SaveChangesAsync();

            // remove sensitive information
            addedContract.Employee = null!;
            addedContract.Company = null!;

            return addedContract;
        }


    

    }
}
