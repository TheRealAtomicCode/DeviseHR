using HR.DTO.Inbound;
using HR.DTO.Outbound;
using Models;

namespace HR.Services.EmployeeService.Interfaces
{
    public interface ICredentialService
    {
        Task<LoginResponse> FindByCredentials(LoginRequest loginRequest);
        Task<Employee> FindAndRefreshEmployeeById(int id);
        Task<LoginResponse> RefreshUserToken(int employeeId, string refreshToken);
        Task LogoutService(int employeeId, string refreshToken);
    }
}
