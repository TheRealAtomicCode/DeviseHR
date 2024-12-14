using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using HR.DTO.Inbound;
using HR.Subroutines;
using HR.DTO.outbound;
using Mapster;

namespace HR.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {


        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public EmployeeRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }

        public async Task<Employee?> GetEmployeeByEmailOrDefault(string email)
        {
            return await _context.Employees
                .Include(u => u.Company)
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(emp => emp.Email == email.Trim().ToLower());
        }

        public async Task<Employee?> GetEmployeeById(int id, int companyId)
        {
            return await _context.Employees
                .Include(u => u.Company)
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(emp => emp.Id == id && emp.CompanyId == companyId);
        }

        public async Task<EmployeeDto?> GetEmployeeDtoById(int employeeId, int companyId)
        {
            var employeeData = await (from e in _context.Employees
                                      where e.Id == employeeId && e.CompanyId == companyId
                                      join h in _context.Hierarchies on e.Id equals h.SubordinateId into hierarchy
                                      from h in hierarchy.DefaultIfEmpty()
                                      join m in _context.Employees on h.ManagerId equals m.Id into managers
                                      from m in managers.DefaultIfEmpty()
                                      select new
                                      {
                                          Employee = e,
                                          Manager = m,
                                          Hierarchies = hierarchy
                                      }).FirstOrDefaultAsync();

            if (employeeData == null) return null;

            var employeeDto = employeeData.Employee.Adapt<EmployeeDto>();
            employeeDto.Managers = employeeData.Hierarchies.Select(h => new ManagerDto
            {
                ManagerId = h.ManagerId,
                FullName = $"{employeeData.Manager.FirstName} {employeeData.Manager.LastName}",
                Title = employeeData.Manager.Title
            }).ToList();

            return employeeDto;
        }


        public async Task<List<Employee>> GetAllEmployees(string email)
        {
            return await _context.Employees.ToListAsync();
        }

        

        public async Task AddEmployee(Employee newEmployee)
        {
            await _context.Employees.AddAsync(newEmployee);
        }

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
