using HR.DTO.Inbound;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<Employee?> GetEmployeeById(int id);
        Task<Employee?> GetEmployeeByEmailOrDefault(string email);
        Task<List<Employee>> GetAllEmployees(string email);
        Task AddEmployee(Employee newEmployee);
        Task AddHierarchy(Hierarchy hierarchy);
        Task SaveChangesAsync();

    }
}
