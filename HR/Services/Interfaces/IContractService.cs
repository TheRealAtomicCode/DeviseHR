using HR.DTO.Inbound;
using Models;

namespace HR.Services.Interfaces
{
    public interface IContractService
    {
        Task<CreateContractDto> CalculateLeaveYear(CreateContractDto newContract, int companyId);
        Task<Contract> CreateContract(CreateContractDto newContract, int myId, int companyId, int userRole);
    }
}
