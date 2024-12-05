using HR.DTO.Inbound;

namespace HR.Services.UserServices.Interfaces
{
    public interface IEmployeeService
    {
        Task<int> CreateEmployee(NewEmployeeDto newEmployee, int myId, int companyId, int userType);
    }
}
