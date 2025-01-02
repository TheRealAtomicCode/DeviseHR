using HR.DTO;
using Microsoft.AspNetCore.JsonPatch;
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
        Task EditLastContract(EditContractRequest editContractRequest, int employeeId, int myId, int myRole, int companyId);


    }
}
