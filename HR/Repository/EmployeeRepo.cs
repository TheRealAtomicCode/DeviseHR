using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

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

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
        }
       


        public async Task<List<Employee>> GetAllEmployees(string email)
        {
            return await _context.Employees.ToListAsync();
        }

        

        public async Task<Employee> AddEmployee(Employee emp)
        {
            throw new NotImplementedException();
        }



        public async Task<Employee> UpdateEmployee(Employee emp)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.Employees.ToListAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
