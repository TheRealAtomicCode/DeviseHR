using HR.DTO;
using Models;
using System.ComponentModel.Design;


namespace HR.Repository.Interfaces
{
    public interface IContractRepo
    {
        Task<Contract?> GetContractByIdOrDefault(int contractId, int companyId);
        Task<List<Contract>> GetContractsThatFallBetweenDates(int employeeId, int companyId, DateOnly startDate, DateOnly endDate);
        Task<Contract?> GetLastContractOrDefault(int employeeId, int companyId);
        Task<Contract> AddContract(Employee employee, AddContractRequest newContract, int myId, int companyId);
        Task<Contract?> GetLastContractByDateOrDefault(int employeeId, int companyId, DateOnly endOfLaveYear);
        Task<Contract?> GetFirstContractOrDefault(int employeeId, int companyId);

        // save
        Task SaveChangesAsync();

    }
}
