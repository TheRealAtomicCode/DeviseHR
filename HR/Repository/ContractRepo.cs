using HR.DTO;
using HR.Repository.Interfaces;
using HR.Subroutines;
using Microsoft.EntityFrameworkCore;
using Models;

namespace HR.Repository
{
    public class ContractRepo : IContractRepo
    {
        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public ContractRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }


        public async Task<Contract?> GetContractByIdOrDefault(int contractId, int companyId)
        {

            Contract? contract = await _context.Contracts
                .Where(c => c.Id == contractId && c.CompanyId == companyId)
                .FirstOrDefaultAsync();

            return contract;
        }


        public async Task<List<Contract>> GetContractsThatFallBetweenDates(int employeeId, int companyId, DateOnly startDate, DateOnly endDate)
        {

            List<Contract> contracts = await _context.Contracts
                .Where(c => c.EmployeeId == employeeId && c.CompanyId == companyId && c.ContractStartDate >= startDate && c.ContractStartDate <= endDate)
                .OrderBy(c => c.ContractStartDate)
                .ToListAsync();

            Contract? contractBeforeFirst;

            /* 
             * 
             * IMPORTANT CONTEXT
            
                Code above: gets the contracts that have started between 2 dates (should be 1 leave year)

                Code bellow: Returns the contacts if the start date is the same as the first contract start date,
                                otherwise get the contract before the arg start date.

                Reason: 
                    if the contract start date is the same as the start date in the methods argument, then you would not need to get any contracts before it,
                    however... if the contract start date is after the methods arg start date, then there might be a contract that was active before it,
                    thus the need to get any conbtract that started before the start date.
                    
                    NOTE: this "first contract" that was selected from the code below needs to be cut in a later method in order to get an accurate represenation of a leave year

                    USE: the VirtualizeContracts() method and the PlaceContractsInYear() method Located in the Subroutines folder.
            */

            if (contracts.Count > 0)
            {
                if (contracts[0].ContractStartDate == startDate) return contracts;
            }

            contractBeforeFirst = await _context.Contracts
                             .Where(c => c.EmployeeId == employeeId && c.CompanyId == companyId &&
                              c.ContractStartDate < startDate)
                             .OrderByDescending(c => c.ContractStartDate)
                             .FirstOrDefaultAsync();

            if (contractBeforeFirst != null)
            {
                contracts.Insert(0, contractBeforeFirst);
            }

            return contracts;
        }

        public async Task<Contract?> GetLastContractOrDefault(int employeeId, int companyId)
        {
            Contract? contract = await _context.Contracts.OrderByDescending(c => c.ContractStartDate)
                .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.CompanyId == companyId);

            return contract;
        }


        public async Task<Contract?> GetLastContractByDateOrDefault(int employeeId, int companyId, DateOnly endOfLaveYear)
        {
            Contract? contract = await _context.Contracts
                .OrderByDescending(c => c.ContractStartDate)
                .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.CompanyId == companyId && c.ContractStartDate <= endOfLaveYear);

            return contract;
        }



        public async Task<Contract?> GetFirstContractOrDefault(int employeeId, int companyId)
        {
            Contract? contract = await _context.Contracts
                .OrderBy(c => c.ContractStartDate)
                .FirstOrDefaultAsync(c => c.EmployeeId == employeeId && c.CompanyId == companyId);

            return contract;
        }

        public async Task<List<Contract>> GetLast2Contracts(int employeeId, int companyId)
        {
            List<Contract> contracts = await _context.Contracts
                .Where(c => c.EmployeeId == employeeId && c.CompanyId == companyId)
                .OrderByDescending(c => c.ContractStartDate)
                .Take(2)
                .ToListAsync();

            return contracts;
        }


        public async Task<Contract> AddContract(Employee employee, AddContractRequest newContract, int companyId, int myId)
        {
            int? workingPatternId = newContract.PatternId == 0 || newContract.PatternId == null ? null : newContract.PatternId;

            Contract contract = new Contract
            {
                EmployeeId = employee.Id,
                CompanyId = companyId,
                ContractStartDate = newContract.ContractStartDate,
                PatternId = workingPatternId,
                AddedBy = myId,
                UpdatedBy = null,
                ContractType = newContract.ContractType,
                ContractedHoursPerWeek = newContract.ContractedHoursPerWeek,
                CompanyHoursPerWeek = newContract.CompanyHoursPerWeek,
                ContractedDaysPerWeek = newContract.ContractedDaysPerWeek,
                CompanyDaysPerWeek = newContract.CompanyDaysPerWeek,
                AverageWorkingDay = newContract.AverageWorkingDay,
                IsDays = newContract.IsDays,
                CompanyLeaveEntitlement = newContract.CompanyLeaveEntitlement,
                ContractedLeaveEntitlement = newContract.ContractedLeaveEntitlement,
                FirstLeaveAllowence = newContract.FirstLeaveAllowence,
                NextLeaveAllowence = newContract.NextLeaveAllowence,
                TermTimeId = newContract.TermTimeId,
                DiscardedId = null,
            };

            await _context.AddAsync(contract);

            return contract;
        }







        // save
        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                SqlExceptionHandler.ExceptionHandler(ex);
            }

        }

      
    }
}
