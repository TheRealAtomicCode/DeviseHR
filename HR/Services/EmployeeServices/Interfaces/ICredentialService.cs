using Models;
using HR.DTO.Inbound;
using HR.DTO.Outbound;

namespace HR.Services.EmployeeServices.Interfaces
{
    public interface ICredentialService
    {
        Task<LoginResponse> FindByCredentialts(LoginRequest loginRequest);
        Task<Employee> FindAndRefreshEmployeeById(int id);
    }
}
