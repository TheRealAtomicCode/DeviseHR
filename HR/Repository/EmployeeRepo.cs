﻿using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;

namespace OP.Repository
{
    public class EmployeeRepo :IEmployeeRepo
    {


        private readonly DeviseHrContext _Context;
        private readonly IConfiguration _configuration;

        public EmployeeRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _Context = dbContext;
            _configuration = configuration;
        }

        public async Task<Employee?> GetEmployeeByEmail(string email)
        {
            return await _Context.Employees.FirstOrDefaultAsync(emp => emp.Email == email);
        }

        public async Task<Employee?> GetEmployeeById(int id)
        {
            return await _Context.Employees.FirstOrDefaultAsync(emp => emp.Id == id);
        }
        public async Task<List<Employee>> GetAllEmployees(string email)
        {
            return await _Context.Employees.ToListAsync();
        }

        public async void IncrementLoginAttemt(Employee emp)
        {
            emp.LoginAttempt++;
            await _Context.SaveChangesAsync();
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