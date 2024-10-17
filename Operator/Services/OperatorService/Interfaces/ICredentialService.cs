using Models;
using OP.DTO.Inbound;

namespace OP.Services.OperatorService.Interfaces
{
    public interface ICredentialService
    {
        Task<Operator> FindByCredentialts(LoginRequest loginRequest);
        Task<Operator> FindAndRefreshOperatorById(int id);
    }
}
