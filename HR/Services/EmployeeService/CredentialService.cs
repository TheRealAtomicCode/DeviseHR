
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Models;
using HR.DTO.Inbound;
using HR.DTO.Outbound;
using HR.Repository.Interfaces;

using HR.Subroutines;
using HR.Utils;
using System.Text.RegularExpressions;
using HR.Services.EmployeeService.Interfaces;

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

            string secret = _configuration["JwtSettings:SecretKey"]!;
            string expTime = _configuration["JwtSettings:ExpTime"]!;

            // generate auth token
            string refreshToken = await Token.GenerateEmployeeJWT(emp, secret, expTime);
            string jwt = await Token.GenerateEmployeeJWT(emp, secret, expTime);

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
