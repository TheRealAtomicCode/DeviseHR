using HR.DTO;
using Models;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;

namespace HR.Services.Interfaces
{
    public interface IContractService
    {
        Task<AddContractRequest> CalculateLeaveYear(AddContractRequest newContract, int companyId);
        Task<ContractDto> CreateContract(AddContractRequest newContract, int myId, int companyId, int userRole);
        Task<LeaveYearResponse> GetLeaveYear(DateOnly leaveYearDate, int userId, int myId, int userType, int companyId);
        Task<ContractDto> DetatchWorkingPattern(int contractId, int myId, int myRole, int companyId);


    }
}
