using Models;
using OP.DTO.Inbound;
using OP.DTO.Outbound;

namespace OP.Services.OperatorService.Interfaces
{
    public interface ICredentialService
    {
        Task<LoginResponse> FindByCredentialts(LoginRequest loginRequest);
        Task<Operator> FindAndRefreshOperatorById(int id);
    }
}
