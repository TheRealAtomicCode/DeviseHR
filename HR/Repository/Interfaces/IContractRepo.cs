using Models;

namespace HR.Repository.Interfaces
{
    public interface IContractRepo
    {
        Task<List<Contract>> GetContractByLeaveYear(Employee employee, DateOnly annualLeaveStartDate);

        // copied
        Task<Employee?> GetEmployeeById(int id, int companyId);
    }
}
