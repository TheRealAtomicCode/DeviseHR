


using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Models;
using OP.DTO.Inbound;
using OP.Repository.Interfaces;
using OP.Services.OperatorService.Interfaces;
using OP.Subroutines;
using OP.Utils;
using System.Text.RegularExpressions;

namespace OP.Services.OperatorService
{
    public class CredentialService : ICredentialService
    {

        private readonly IOperatorRepo _operatorRepo;
        private readonly IConfiguration _configuration;

        public CredentialService(IOperatorRepo operatorRepo, IConfiguration configuration)
        {
            _operatorRepo = operatorRepo;
            _configuration = configuration;
        }

        public async Task<Operator> FindByCredentialts(LoginRequest loginRequest)
        {
            loginRequest.Email.Trim().ToLower();

            Operator? op = await _operatorRepo.GetOperatorByEmail(loginRequest.Email);

            if (op == null || op.PasswordHash == null) throw new Exception("Incorrect Email or Password");

            bool isMatch = PasswordUtils.IsMatch(loginRequest.Password, op.PasswordHash);

            if (!isMatch)
            {
                _operatorRepo.IncrementLoginAttemt(op);
                throw new Exception("Invalid Email or Password");
            }

            string loginAttempLimit = _configuration["JwtSettings:LoginAttempLimit"]!;
            Verify.OperatorAccess(op, loginAttempLimit);

            string secret = _configuration["JwtSettings:SecretKey"]!;
            string expTime = _configuration["JwtSettings:ExpTime"]!;
            await Token.GenerateOperatorJWT(op, secret, expTime);

            // generate auth token
            string jwt = await Token.GenerateOperatorJWT(op, secret, expTime);

            // Generate DTO

            return op;
        }


        public Task<Operator> FindAndRefreshOperatorById(int id)
        {
            throw new NotImplementedException();
        }



    }
}
