using HR.DTO.Inbound;
using Models;
using System.ComponentModel.Design;


namespace HR.Repository.Interfaces
{
    public interface IContractRepo
    {
        Task<List<Contract>> GetContractsByLeaveYear(int employeeId, int companyId, DateOnly annualLeaveStartDate);
        Task<Contract?> GetLastContractOrDefault(int employeeId, int companyId);
        Task<Contract> AddContract(Employee employee, CreateContractDto newContract, int myId, int companyId);
        Task<Contract?> GetLastContractByDate(int employeeId, int companyId, DateOnly providedDate);
        Task<Contract?> GetFirstContractOrDefault(int employeeId, int companyId);

        // hirrarchy related
        Task<bool> HasManager(int subordinateId);
        Task<bool> IsRelated(int managerId, int subordinateId);

        // save
        Task SaveChangesAsync();

        // copied
        Task<Employee?> GetEmployeeById(int id, int companyId);
    }
}
