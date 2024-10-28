using Models;

namespace HR.Repository.Interfaces
{
    public interface IEmployeeRepo
    {
        Task<Employee?> GetEmployeeById(int id);
        Task<Employee?> GetEmployeeByEmailOrDefault(string email);
        Task<List<Employee>> GetAllEmployees(string email);
        Task<Employee> AddEmployee(Employee op);
        Task<Employee> UpdateEmployee(Employee op);
        Task<bool> SaveChangesAsync();

    }
}
