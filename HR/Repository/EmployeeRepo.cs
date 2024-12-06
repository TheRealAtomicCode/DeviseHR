using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using HR.DTO.Inbound;

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

        

        public async Task AddEmployee(Employee newEmployee, int myId, int companyId)
        {
            await _context.Employees.AddAsync(newEmployee);
        }



        public async Task<bool> SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
