using HR.DTO.Inbound;
using HR.Repository.Interfaces;
using HR.Services.UserServices.Interfaces;


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

        public async Task<int> CreateEmployee(NewEmployeeDto newEmployee, int myId, int companyId, int userType)
        {
            

            return 1;
        }
    }
}
