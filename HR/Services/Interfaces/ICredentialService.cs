using HR.DTO;
using Models;
using System.ComponentModel.Design;

namespace HR.Services.Interfaces
{
    public interface ICredentialService
    {
        Task<LoginResponse> FindByCredentials(LoginRequest loginRequest);
        Task<Employee> FindAndRefreshEmployeeById(int id);
        Task<LoginResponse> RefreshUserToken(int employeeId, int companyId, string refreshToken);
        Task LogoutService(int employeeId, int companyId, string refreshToken);
        Task ResetPasswordService(string email, bool isNewUser);
        Task<LoginResponse> ConfirmVerificationCodeByEmail(string email, string verificationCode, string newPassword, bool isNewUser);
    }
}
