using HR.DTO.Inbound;
using HR.DTO.Outbound;
using Models;

namespace HR.Services.UserServices.Interfaces
{
    public interface ICredentialService
    {
        Task<LoginResponse> FindByCredentials(LoginRequest loginRequest);
        Task<Employee> FindAndRefreshEmployeeById(int id);
        Task<LoginResponse> RefreshUserToken(int employeeId, string refreshToken);
        Task LogoutService(int employeeId, string refreshToken);
        Task ResetPasswordService(string email, bool isNewUser);
        Task<LoginResponse> ConfirmVerificationCodeByEmail(string email, string verificationCode, string newPassword, bool isNewUser);
    }
}
