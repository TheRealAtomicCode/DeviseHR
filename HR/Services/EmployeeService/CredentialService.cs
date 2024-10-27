
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Models;
using HR.DTO.Inbound;
using HR.DTO.Outbound;
using HR.Repository.Interfaces;
using HR.Utils;
using System.Text.RegularExpressions;
using HR.Services.EmployeeService.Interfaces;
using HR.Subroutines;
using HR.Repository;
using System.Net.Sockets;

namespace HR.Services.EmployeeService
{
    public class CredentialService : ICredentialService
    {

        private readonly IEmployeeRepo _employeeRepo;
        private readonly IConfiguration _configuration;

        public CredentialService(IEmployeeRepo employeeRepo, IConfiguration configuration)
        {
            _employeeRepo = employeeRepo;
            _configuration = configuration;
        }

        public async Task<LoginResponse> FindByCredentials(LoginRequest loginRequest)
        {
            Employee? emp = await _employeeRepo.GetEmployeeByEmail(loginRequest.Email);

            if (emp == null || emp.PasswordHash == null) throw new Exception("Incorrect Email or Password");

            bool isMatch = PasswordUtils.IsMatch(loginRequest.Password, emp.PasswordHash);

            if (!isMatch)
            {
                _employeeRepo.IncrementLoginAttemt(emp);
                throw new Exception("Invalid Email or Password");
            }

            Verify.EmployeeAccess(emp, _configuration, false);

            // generate auth token
            string jwt = await Token.GenerateEmployeeJWT(emp, _configuration, false);
            string refreshToken = await Token.GenerateEmployeeJWT(emp, _configuration, true);

            // add new refreshToken
            await _employeeRepo.AddRefreshToken(emp, refreshToken);

            var empDto = emp.Adapt<LoginResponse>();

            empDto.Jwt = jwt;
            empDto.RefreshToken = refreshToken;

            return empDto;
        }


        public async Task<LoginResponse> RefreshUserToken(int employeeId, string refreshToken)
        {
            Employee? emp = await _employeeRepo.GetEmployeeById(employeeId);

            if(emp == null) throw new Exception("Unable to locate Employee");

            if (!emp.RefreshTokens.Contains(refreshToken)) throw new Exception("Please authenticate");

            Verify.EmployeeAccess(emp, _configuration, false);

            string jwt = await Token.GenerateEmployeeJWT(emp, _configuration, true);
            string newRefreshToken = await Token.GenerateEmployeeJWT(emp, _configuration, true);

            await _employeeRepo.UpdateRefreshToken(emp, refreshToken, newRefreshToken);

            var empDto = emp.Adapt<LoginResponse>();

            empDto.Jwt = jwt;
            empDto.RefreshToken = newRefreshToken;

            return empDto;
        }


        public async Task LogoutService(int employeeId, string refreshToken)
        {
            Employee? emp = await _employeeRepo.GetEmployeeById(employeeId);

            if (emp == null) throw new Exception("User not found");

            if (refreshToken == string.Empty)
            {
                await _employeeRepo.ClearRefreshTokens(emp);
            }
            else
            {
                await _employeeRepo.RemoveSingleRefreshToken(emp, refreshToken);
            }
            
        }


        public async Task ResetPasswordService(string email, bool isNewUser)
        {
            string verificationCode = StringUtils.GenerateSixDigitString();

            Employee? emp = await _employeeRepo.GetEmployeeByEmail(email);

            if (emp == null) throw new Exception("Incorrect email");

            Verify.EmployeeAccess(emp, _configuration, false);

            await _employeeRepo.UpdateVerificationCode(emp, verificationCode);

            // send verifivation code email
        }


        public async Task<LoginResponse> ConfirmVerificationCodeByEmail(string email, string verificationCode, string newPassword, bool isNewUser)
        {
            StringUtils.ValidateEmail(email);
            StringUtils.ValidateStrongPassword(newPassword);

            Employee? emp = await _employeeRepo.GetEmployeeByEmail(email);

            if (emp == null) throw new Exception("Invalid user credencials");

            if (emp.VerificationCode != verificationCode)
            {
                await _employeeRepo.UpdateVerificationCodeAfterConfermation(emp, "", "");
                throw new Exception("Invalid verification code");
            }

            // generate password hash
            string passwordHash = PasswordUtils.GenerateHash(newPassword);

            Verify.EmployeeAccess(emp, _configuration, false);

            string logintimeExpiration = _configuration["login:ExpTime"]!;

            DateTime currentTime = DateTime.Now; // Current time
            DateTime expiresAt = currentTime.AddMinutes(int.Parse(logintimeExpiration));
            if (emp.LastLoginTime > expiresAt) throw new Exception("Verifivation code has expired");

            string jwt = await Token.GenerateEmployeeJWT(emp, _configuration, true);
            string refreshToken = await Token.GenerateEmployeeJWT(emp, _configuration, true);

            await _employeeRepo.UpdateVerificationCodeAfterConfermation(emp, passwordHash, refreshToken);

            var empDto = emp.Adapt<LoginResponse>();

            empDto.Jwt = jwt;
            empDto.RefreshToken = refreshToken;

            return empDto;

        }



        public Task<Employee> FindAndRefreshEmployeeById(int id)
        {
            throw new NotImplementedException();
        }



    }
}
