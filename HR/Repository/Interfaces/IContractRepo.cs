using HR.DTO.Inbound;
using Models;
using System.ComponentModel.Design;


namespace HR.Repository.Interfaces
{
    public interface IContractRepo
    {
        Task<List<Contract>> GetContractByLeaveYear(Employee employee, DateOnly annualLeaveStartDate);
        Task<Contract?> GetLastContractOrDefault(Employee employee);
        Task<Contract> AddContract(Employee employee, CreateContractDto newContract, int myId, int companyId);
        Task<Contract?> GetLastContractByDateOrDefault(Employee employee, DateOnly providedDate);

        // hirrarchy related
        Task<bool> HasManager(int subordinateId);
        Task<bool> IsRelated(int managerId, int subordinateId);

        // save
        Task SaveChangesAsync();

        // copied
        Task<Employee?> GetEmployeeById(int id, int companyId);
    }
}
