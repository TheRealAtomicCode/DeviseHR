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

            if (contracts.Count == 0)
            {
                contractBeforeFirst = await _context.Contracts
                        .Where(c => c.EmployeeId == employee.Id && c.CompanyId == employee.CompanyId &&
                         c.ContractStartDate < annualLeaveStartDate)
                        .OrderByDescending(c => c.ContractStartDate)
                        .FirstOrDefaultAsync();

                if (contractBeforeFirst != null)
                {
                    contracts.Add(contractBeforeFirst);
                }
            }
            else
            {
                if (contracts[0].ContractStartDate > employee.AnnualLeaveStartDate)
                {
                    contractBeforeFirst = await _context.Contracts
                        .Where(c => c.EmployeeId == employee.Id && c.CompanyId == employee.CompanyId &&
                         c.ContractStartDate < annualLeaveStartDate)
                        .OrderByDescending(c => c.ContractStartDate)
                        .FirstOrDefaultAsync();

                    if (contractBeforeFirst != null)
                    {
                        contracts.Add(contractBeforeFirst);
                    }
                }
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
