using HR.DTO.Inbound;
using HR.DTO.outbound;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<EmployeeDto?> GetEmployeeDtoById(int id, int companyId);
        Task<Employee?> GetEmployeeById(int id);
        Task<Employee?> GetEmployeeByEmailOrDefault(string email);
        Task<List<Employee>> GetAllEmployees(string email);
        Task AddEmployee(Employee newEmployee);
        Task SaveChangesAsync();

    }
}
