﻿using HR.DTO.Inbound;
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

        public async Task<Contract?> GetLastContractOrDefault(Employee employee)
        {
            Contract? contract = await _context.Contracts.OrderByDescending(c => c.ContractStartDate)
                .FirstOrDefaultAsync(c => c.EmployeeId == employee.Id && c.CompanyId == employee.CompanyId);

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


        // hierarchy related
        public async Task<bool> HasManager(int subordinateId)
        {
            var hierarchy = await _context.Hierarchies.FirstOrDefaultAsync(s => s.SubordinateId == subordinateId);

            bool hasManager = hierarchy != null;

            return hasManager;
        }

        public async Task<bool> IsRelated(int managerId, int subordinateId)
        {
            var hierarchy = await _context.Hierarchies.FirstOrDefaultAsync(s => s.ManagerId == managerId && s.SubordinateId == subordinateId);

            bool isRelated = hierarchy != null;

            return isRelated;
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
