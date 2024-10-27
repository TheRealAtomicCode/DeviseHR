using HR.DTO.Inbound;
using HR.DTO.Outbound;
using Models;

namespace HR.Services.EmployeeService.Interfaces
{
    public interface ICredentialService
    {
        Task<LoginResponse> FindByCredentials(LoginRequest loginRequest);
        Task<Employee> FindAndRefreshEmployeeById(int id);
    }
}
