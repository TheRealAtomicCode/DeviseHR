using HR.DTO.Inbound;
using HR.DTO.outbound;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HR.Services.UserServices.Interfaces
{
    public interface IEmployeeService
    {
        Task<int> CreateEmployee(NewEmployeeDto newEmployee, int myId, int companyId, int myRole);
        Task<EmployeeDto> GetEmployee(int employeeId, int myId, int companyId, int myRole);
        Task<List<FoundEmployee>> GetAllEmployees(string searchTerm, int page, int skip, int myId, int companyId, int myRole, bool enableShowEmployees);
    }
}
