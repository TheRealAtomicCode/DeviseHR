using HR.DTO;
using Models;

namespace HR.Services.Interfaces
{
    public interface IContractService
    {
        Task<AddContractRequest> CalculateLeaveYear(AddContractRequest newContract, int companyId);
        Task<ContractDto> CreateContract(AddContractRequest newContract, int myId, int companyId, int userRole);
        Task<LeaveYearResponse> GetLeaveYear(DateOnly leaveYearDate, int userId, int myId, int userType, int companyId);


    }
}
