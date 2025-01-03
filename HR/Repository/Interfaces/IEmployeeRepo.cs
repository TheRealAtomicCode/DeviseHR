﻿
using HR.DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.ComponentModel.Design;

namespace HR.Repository.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<EmployeeDto> GetEmployeeDtoById(int id, int companyId);
        Task<Employee?> GetEmployeeById(int id, int companyId);
        Task<Employee?> GetEmployeeByEmailOrDefault(string email);
        Task<List<FoundEmployee>> GetAllEmployeesByName(string? searchTerm, int? page, int? skip, int companyId, int? myId);
        Task AddEmployee(Employee newEmployee);
        Task<List<EmployeeWithContractDto>> GetEmployeesByWorkingPatternId(int workingPatternId, string? searchTerm, int? page, int? skip, int companyId, int? myId);
        Task SaveChangesAsync();


    }
}
