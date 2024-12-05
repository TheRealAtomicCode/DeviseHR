using Mapster;
using Models;
using Op.Repository.IRepostiory;
using OP.DTO.Inbound;
using OP.DTO.Outbound;
using OP.Services.Interfaces;
using OP.Subroutines;
using OP.Utils;

namespace OP.Services.CredentialService
{
    public class CredentialService : ICredentialService
    {
        private readonly DeviseHrContext _context;
        private readonly IGenericRepository<Operator> _operatorRepo;
        private readonly IConfiguration _configuration;

        public CredentialService(IGenericRepository<Operator> operatorRepo, IConfiguration configuration, DeviseHrContext context)
        {
            _operatorRepo = operatorRepo;
            _configuration = configuration;
            _context = context;
        }

        public async Task<LoginResponse> FindByCredentialts(LoginRequest loginRequest)
        {
            loginRequest.Email.Trim().ToLower();

            Operator? op = await _operatorRepo.GetAsync(op => op.Email == loginRequest.Email);

            if (op == null || op.PasswordHash == null) throw new Exception("Incorrect Email or Password");

            bool isMatch = PasswordUtils.IsMatch(loginRequest.Password, op.PasswordHash);

            if (!isMatch)
            {
                IncrementLoginAttemt(op);
                throw new Exception("Invalid Email or Password");
            }

            string loginAttempLimit = _configuration["JwtSettings:LoginAttempLimit"]!;
            Verify.OperatorAccess(op, loginAttempLimit);

            string secret = _configuration["JwtSettings:SecretKey"]!;
            string expTime = _configuration["JwtSettings:ExpTime"]!;

            // generate auth token
            string refreshToken = await Token.GenerateOperatorJWT(op, secret, expTime);
            string jwt = await Token.GenerateOperatorJWT(op, secret, expTime);

            var opDto = op.Adapt<LoginResponse>();

            opDto.Jwt = jwt;
            opDto.RefreshToken = refreshToken;

            return opDto;
        }

        public async void IncrementLoginAttemt(Operator op)
        {
            op.LoginAttempt++;
            await _context.SaveChangesAsync();
        }

        public Task<Operator> FindAndRefreshOperatorById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> FindByCredentials(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }
    }
}
