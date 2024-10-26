


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
using HR.Services.EmployeeServices.Interfaces;

namespace OP.Services.EmployeeServices
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

        public async Task<LoginResponse> FindByCredentialts(LoginRequest loginRequest)
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
            Verify.OperatorAccess(emp, loginAttempLimit);

            string secret = _configuration["JwtSettings:SecretKey"]!;
            string expTime = _configuration["JwtSettings:ExpTime"]!;

            // generate auth token
            string refreshToken = await Token.GenerateOperatorJWT(emp, secret, expTime);
            string jwt = await Token.GenerateOperatorJWT(emp, secret, expTime);

            var opDto = emp.Adapt<LoginResponse>();

            opDto.Jwt = jwt;
            opDto.RefreshToken = refreshToken;

            return opDto;
        }

        public Task<Employee> FindAndRefreshEmployeeById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
