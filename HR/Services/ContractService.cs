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

        //    bool isDays = ContractVerification.CheckAllContractsHaveSameLeaveUnit(existingContracts, newContract);

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

            var employee = await _contractRepo.GetEmployeeById(newContract.employeeId, companyId);
            if (employee == null) throw new Exception("Employee not found");

            DateOnly annualLeaveStartDate = DateModifier.GetLeaveYearStartDate(newContract.StartDate, employee.AnnualLeaveStartDate);

            bool isDays = true;

            NewContractCalculationResult newContractResult = CalculateContract.CalculateNewContractEntitlementMut(newContract, annualLeaveStartDate, isDays);

            // double thisLeaveYearsAllowance = previousLeaveYearEntitlement + newContractResult.ThisContractEntitlement;
            double thisLeaveYearsAllowance = newContractResult.ThisContractEntitlement;

            newContract.FirstLeaveAllowence = (int)Math.Ceiling(thisLeaveYearsAllowance);
            newContract.NextLeaveAllowence = (int)Math.Ceiling(newContractResult.ContractleaveEntitlementPerYear);
            newContract.ContractedLeaveEntitlement = (int)Math.Ceiling(newContractResult.ContractleaveEntitlementPerYear);

            return newContract;
        }

    }
}
