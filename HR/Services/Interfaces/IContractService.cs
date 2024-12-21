using HR.DTO.Inbound;
using HR.DTO.outbound;
using Models;

namespace HR.Services.Interfaces
{
    public interface IContractService
    {
        Task<CreateContractDto> CalculateLeaveYear(CreateContractDto newContract, int companyId);
        Task<Contract> CreateContract(CreateContractDto newContract, int myId, int companyId, int userRole);
        Task<ContractAndLeaveYearCount> GetLeaveYear(DateOnly leaveYearDate, int userId, int myId, int userType, int companyId);


    }
}
