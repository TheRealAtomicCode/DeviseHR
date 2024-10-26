using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;

namespace HR.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {


        private readonly DeviseHrContext _dbContext;
        private readonly IConfiguration _configuration;

        public EmployeeRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<Employee?> GetEmployeeByEmail(string email)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
        }
        public async Task<List<Employee>> GetAllEmployees(string email)
        {
            return await _dbContext.Employees.ToListAsync();
        }

        public async void IncrementLoginAttemt(Employee emp)
        {
            emp.LoginAttempt++;
            await _dbContext.SaveChangesAsync();
        }








        public async Task<Employee> AddEmployee(Employee emp)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> DeleteEmployee(Employee emp)
        {
            throw new NotImplementedException();
        }





        public async Task<Employee> UpdateEmployee(Employee emp)
        {
            throw new NotImplementedException();
        }
    }
}
