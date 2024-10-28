
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Models;
using HR.DTO.Inbound;
using HR.DTO.Outbound;
using HR.Repository.Interfaces;

using System.Text.RegularExpressions;
using HR.Services.EmployeeService.Interfaces;
using HR.Subroutines;
using HR.Repository;
using System.Net.Sockets;
using Common;
using System.Security.Claims;


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
            Employee? emp = await _employeeRepo.GetEmployeeByEmailOrDefault(loginRequest.Email);

            if (emp == null || emp.PasswordHash == null) throw new Exception("Incorrect Email or Password");

            bool isMatch = PasswordUtils.IsMatch(loginRequest.Password, emp.PasswordHash);

            if (!isMatch)
            {
                emp.LoginAttempt++;
                await _employeeRepo.SaveChangesAsync();
                throw new Exception("Incorrect email or password");
            }

            Verify.EmployeeAccess(emp, _configuration, false);

            var tokenClaims = GenerateClaims.GetEmployeeJwtClaims(emp);
            var refreshTokenClaims = GenerateClaims.GetEmployeeRefreshTokenClaims(emp);

            // generate auth token
            string jwt = await Token.GenerateJWT(_configuration, "token", tokenClaims);
            string refreshToken = await Token.GenerateJWT(_configuration, "refreshToken", refreshTokenClaims);

            // add refresh token
            if (emp.RefreshTokens.Count >= 6)
            {  // remove token if above 6
                emp.RefreshTokens.RemoveAt(emp.RefreshTokens.Count - 1);
            }
            emp.RefreshTokens.Insert(0, jwt); // insert at index 0

            await _employeeRepo.SaveChangesAsync();

            var empDto = emp.Adapt<LoginResponse>();

            empDto.Jwt = jwt;
            empDto.RefreshToken = refreshToken;

            return empDto;
        }


        public async Task<LoginResponse> RefreshUserToken(int employeeId, string oldRefreshToken)
        {
            Employee? emp = await _employeeRepo.GetEmployeeById(employeeId);

            if(emp == null) throw new Exception("Unable to locate Employee");

            if (!emp.RefreshTokens.Contains(oldRefreshToken)) throw new Exception("Please authenticate");

            Verify.EmployeeAccess(emp, _configuration, false);

            var tokenClaims = new List<Claim>
                {
                    new Claim("id", emp.Id.ToString()),
                    new Claim("userRole", emp.UserRole.ToString()),
                };

            var refreshTokenClaims = new List<Claim>
                {
                    new Claim("id", emp.Id.ToString()),
                    new Claim("userRole", emp.UserRole.ToString()),
                };

            string jwt = await Token.GenerateJWT(_configuration, "token", tokenClaims);
            string newRefreshToken = await Token.GenerateJWT(_configuration, "refreshToken", refreshTokenClaims);

            emp.RefreshTokens = emp.RefreshTokens
                .Select(token => token == oldRefreshToken ? newRefreshToken : token)
                .ToList();

            await _employeeRepo.SaveChangesAsync();

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
                emp.RefreshTokens.Clear();
            }
            else
            {
                emp.RefreshTokens.Remove(refreshToken);
            }

            await _employeeRepo.SaveChangesAsync();

        }


        public async Task ResetPasswordService(string email, bool isNewUser)
        {
            string verificationCode = StringUtils.GenerateSixDigitString();

            Employee? emp = await _employeeRepo.GetEmployeeByEmailOrDefault(email);

            if (emp == null) throw new Exception("Incorrect email");

            Verify.EmployeeAccess(emp, _configuration, false);

            // update verification code
            emp.VerificationCode = verificationCode;
            emp.LastLoginTime = DateTime.Now;

            await _employeeRepo.SaveChangesAsync();

            // send verifivation code email
        }


        public async Task<LoginResponse> ConfirmVerificationCodeByEmail(string email, string verificationCode, string newPassword, bool isNewUser)
        {
            StringUtils.ValidateEmail(email);
            StringUtils.ValidateStrongPassword(newPassword);

            Employee? emp = await _employeeRepo.GetEmployeeByEmailOrDefault(email);

            if (emp == null) throw new Exception("Invalid user credencials");

            if (emp.VerificationCode != verificationCode)
            {
                emp.VerificationCode = null;
                await _employeeRepo.SaveChangesAsync();
                throw new Exception("Invalid verification code");
            }

            // generate password hash
            string passwordHash = PasswordUtils.GenerateHash(newPassword);

            Verify.EmployeeAccess(emp, _configuration, false);

            string logintimeExpiration = _configuration["login:ExpTime"]!;

            DateTime currentTime = DateTime.Now; // Current time
            DateTime expiresAt = currentTime.AddMinutes(int.Parse(logintimeExpiration));
            if (emp.LastLoginTime > expiresAt) throw new Exception("Verifivation code has expired");

            var tokenClaims = new List<Claim>
                {
                    new Claim("id", emp.Id.ToString()),
                    new Claim("userRole", emp.UserRole.ToString()),
                };

            var refreshTokenClaims = new List<Claim>
                {
                    new Claim("id", emp.Id.ToString()),
                    new Claim("userRole", emp.UserRole.ToString()),
                };

            string jwt = await Token.GenerateJWT(_configuration, "token", tokenClaims);
            string refreshToken = await Token.GenerateJWT(_configuration, "refreshToken", refreshTokenClaims);

            // Update Verification Code After Confermation
            emp.IsVerified = true;
            emp.RefreshTokens.Add(refreshToken);
            emp.PasswordHash = passwordHash;
         

            emp.VerificationCode = null;


            await _employeeRepo.SaveChangesAsync();

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
