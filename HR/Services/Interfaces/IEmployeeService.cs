﻿using HR.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;

namespace HR.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<int> CreateEmployee(NewEmployeeRequest newEmployee, int myId, int companyId, int myRole);
        Task<EmployeeDto> GetEmployee(int employeeId, int myId, int companyId, int myRole);
        Task<List<FoundEmployee>> GetAllEmployees(string? searchTerm, int? page, int? skip, int myId, int companyId, int myRole, bool enableShowEmployees);
        Task EditEmployee(JsonPatchDocument<EditEmployeeRequest> patchDoc, int employeeId, int myId, int companyId);
        
    }
}
