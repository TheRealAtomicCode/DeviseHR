using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;

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
        //        .AsNoTracking()
                .FirstOrDefaultAsync(emp => emp.Email == email.Trim().ToLower());
        }

        public void Update(Employee employee)
        {
            _context.Update(employee);
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
        }
       


        public async Task<List<Employee>> GetAllEmployees(string email)
        {
            return await _context.Employees.ToListAsync();
        }

        

        public async Task<int> AddEmployee(Employee newEmployee, int myId, int companyId)
        {

            var employee = new Employee
            {
                FirstName = newEmployee.FirstName,
                LastName = newEmployee.LastName,
                Email = newEmployee.Email,
                UserRole = newEmployee.UserRole,
                PermissionId = newEmployee.PermissionId,
                DateOfBirth = newEmployee.DateOfBirth,
                AnnualLeaveStartDate = newEmployee.AnnualLeaveStartDate,
                AddedByUser = myId,
                CompanyId = companyId,
            };

            _context.Employees.Add(employee);

            return employee.Id;
        }



        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
