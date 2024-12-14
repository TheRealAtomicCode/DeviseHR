using HR.DTO.Inbound;
using HR.DTO.outbound;

namespace HR.Services.UserServices.Interfaces
{
    public interface IEmployeeService
    {
        Task<int> CreateEmployee(NewEmployeeDto newEmployee, int myId, int companyId, int myRole);
        Task<EmployeeDto> GetEmployee(int employeeId, int myId, int companyId, int myRole);
    }
}
