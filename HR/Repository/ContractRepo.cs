using HR.Repository.Interfaces;
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


        public async Task<List<Contract>> GetContractByLeaveYear(Employee employee, DateOnly annualLeaveStartDate)
        {

            DateOnly annualLeaveEndDate = annualLeaveStartDate.AddYears(1);

            List<Contract> contracts = await _context.Contracts
                .Where(c => c.EmployeeId == employee.Id && c.CompanyId == employee.CompanyId && c.ContractStartDate >= annualLeaveStartDate && c.ContractStartDate <= annualLeaveEndDate)
                .OrderByDescending(c => c.ContractStartDate)
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
                             .Where(c => c.EmployeeId == employee.Id && c.CompanyId == employee.CompanyId &&
                              c.ContractStartDate < annualLeaveStartDate)
                             .OrderByDescending(c => c.ContractStartDate)
                             .FirstOrDefaultAsync();

            if (contractBeforeFirst != null)
            {
                contracts.Add(contractBeforeFirst);
            }

            return contracts;
        }



        // coplied
        public async Task<Employee?> GetEmployeeById(int id, int companyId)
        {
            return await _context.Employees
                .Include(u => u.Company)
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(emp => emp.Id == id && emp.CompanyId == companyId);
        }
    }
}
