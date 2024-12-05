using Common;
using HR.DTO.Inbound;
using HR.Repository.Interfaces;
using HR.Services.UserServices.Interfaces;
using Mapster;
using Models;


namespace HR.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IConfiguration _configuration;

        public EmployeeService(IEmployeeRepo employeeRepo, IConfiguration configuration)
        {
            _employeeRepo = employeeRepo;
            _configuration = configuration;
        }

        public async Task<int> CreateEmployee(NewEmployeeDto newEmployee, int myId, int companyId, int myRole)
        {
            StringUtils.ValidateNonEmptyStrings([newEmployee.FirstName, newEmployee.LastName]);
            StringUtils.ValidateEmail(newEmployee.Email);

            var employee = newEmployee.Adapt<Employee>();
            employee.CompanyId = companyId;
            employee.AddedByUser = myId;
            employee.RefreshTokens = [];

            await _employeeRepo.AddEmployee(employee, myId, companyId);

            await _employeeRepo.SaveChangesAsync();

            return employee.Id;
        }
    }
}
