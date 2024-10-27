
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
            loginRequest.Email.Trim().ToLower();

            Employee? emp = await _employeeRepo.GetEmployeeByEmail(loginRequest.Email);

            if (emp == null || emp.PasswordHash == null) throw new Exception("Incorrect Email or Password");

            bool isMatch = PasswordUtils.IsMatch(loginRequest.Password, emp.PasswordHash);

            if (!isMatch)
            {
                _employeeRepo.IncrementLoginAttemt(emp);
                throw new Exception("Invalid Email or Password");
            }

            string loginAttempLimit = _configuration["JwtSettings:LoginAttempLimit"]!;
            Verify.EmployeeAccess(emp, loginAttempLimit);

            string jwtSecret = _configuration["JwtSettings:SecretKey"]!;
            string refreshSecret = _configuration["refreshSettings:SecretKey"]!;
            string expTime = _configuration["JwtSettings:ExpTime"]!;

            // generate auth token
            string jwt = await Token.GenerateEmployeeJWT(emp, jwtSecret, expTime);
            string refreshToken = await Token.GenerateEmployeeJWT(emp, refreshSecret, expTime);

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

            string loginAttempLimit = _configuration["JwtSettings:LoginAttempLimit"]!;
            string jwtSecret = _configuration["JwtSettings:SecretKey"]!;
            string refreshSecret = _configuration["refreshSettings:SecretKey"]!;
            string expTime = _configuration["JwtSettings:ExpTime"]!;

            Verify.EmployeeAccess(emp, loginAttempLimit);

            string jwt = await Token.GenerateEmployeeJWT(emp, jwtSecret, expTime);
            string newRefreshToken = await Token.GenerateEmployeeJWT(emp, refreshSecret, expTime);

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

  

        public Task<Employee> FindAndRefreshEmployeeById(int id)
        {
            throw new NotImplementedException();
        }



    }
}
