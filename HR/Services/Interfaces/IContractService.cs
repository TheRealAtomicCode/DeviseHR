using HR.DTO.Inbound;

namespace HR.Services.Interfaces
{
    public interface IContractService
    {
        Task<CreateContractDto> CalculateLeaveYear(CreateContractDto newConract, int companyId);
    }
}
