using Common;
using HR.DTO.Inbound;
using HR.Repository.Interfaces;
using HR.Services.UserServices.Interfaces;
using HR.Subroutines;
using Mapster;
using Microsoft.EntityFrameworkCore;
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
            newEmployee.AnnualLeaveStartDate = DateModifier.SetYearTo1900(newEmployee.AnnualLeaveStartDate);

            var employee = newEmployee.Adapt<Employee>();
            employee.CompanyId = companyId;
            employee.AddedByUser = myId;
            employee.RefreshTokens = [];

            if (myRole >= 3 && newEmployee.UserRole <= 3) throw new Exception("You can not add managers or admins if your role is a manager"); 

            await _employeeRepo.AddEmployee(employee);

            if (myRole >= 3)
            {
                Hierarchy hierarchy = new Hierarchy
                {
                    ManagerId = myId,
                    SubordinateId = employee.Id
                };

                await _employeeRepo.AddHierarchy(hierarchy);
            }

            if (newEmployee.RegisterUser == true)
            {
                string otp = StringUtils.GenerateSixDigitString();
                // send verification code
                employee.VerificationCode = otp;
            }
            
            await _employeeRepo.SaveChangesAsync();
            

            return employee.Id;
        }


    }
}
