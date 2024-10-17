


using Models;
using OP.DTO.Inbound;
using OP.Repository.Interfaces;
using OP.Services.OperatorService.Interfaces;

namespace OP.Services.OperatorService
{
    public class CredentialService : ICredentialService
    {

        private readonly IOperatorRepo _operatorRepo;

        public CredentialService(IOperatorRepo operatorRepo)
        {
            _operatorRepo = operatorRepo;
        }

      
        public async Task<Operator> FindByCredentialts(LoginRequest loginRequest)
        {
            loginRequest.Email.Trim().ToLower();

            Operator? op = await _operatorRepo.GetOperatorByEmail(loginRequest.Email);

            if (op == null) throw new Exception("Incorrect Email or Password");

            // test if the Password is correct

            // generate auth token

            // Generate DTO

            return op;
        }


        public Task<Operator> FindAndRefreshOperatorById(int id)
        {
            throw new NotImplementedException();
        }



    }
}
