using HR.DTO.Inbound;
using HR.DTO.outbound;
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


        public async Task<List<Contract>> GetContractsThatFallInDates(int employeeId, int compayId, DateOnly startDate, DateOnly endDate)
        {

            List<Contract> contracts = await _context.Contracts
                .Where(c => c.EmployeeId == employeeId && c.CompanyId == compayId && c.ContractStartDate >= startDate && c.ContractStartDate <= endDate)
                .OrderBy(c => c.ContractStartDate)
                .ToListAsync();

            Contract? contractBeforeFirst;

            /* 
             * 
             * IMPORTANT CONTEXT
            
                This code does the following:
            
                -- it gets the contract before the list of contracts that were selected between 2 dates (the annual leave start and end date)

                -- it is important to get that contract as the contract that was selected may have started after the annual leave start date

                -- thus not selectng an entire year, but a partial year

                -- also if no contracts were found, it could be due to an existing contract that has started before the annual; leave start date,
                -- and the employee has been on that contact ever sinse.

                if a contract was found, it must be appended to the start of the list of contracts, then cut off later

            */
            contractBeforeFirst = await _context.Contracts
                             .Where(c => c.EmployeeId == employeeId && c.CompanyId == compayId &&
                              c.ContractStartDate < startDate)
                             .OrderByDescending(c => c.ContractStartDate)
                             .FirstOrDefaultAsync();

            if (contractBeforeFirst != null)
            {
                contracts.Add(contractBeforeFirst);
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


        public async Task<Contract> AddContract(Employee employee, CreateContractDto newContract, int companyId, int myId)
        {
            Contract contract = new Contract
            {
                EmployeeId = employee.Id,
                CompanyId = companyId,
                ContractStartDate = newContract.ContractStartDate,
                PatternId = null,
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
