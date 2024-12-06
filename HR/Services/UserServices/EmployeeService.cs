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

            await _employeeRepo.AddEmployee(employee, myId, companyId);

            if (newEmployee.RegisterUser == true)
            {
                string otp = StringUtils.GenerateSixDigitString();
                // send verification code
                employee.VerificationCode = otp;
            }
            
            try
            {
                await _employeeRepo.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                SqlExceptionHandler.ExceptionHandler(ex, "employee_email_key");
            }

            return employee.Id;
        }


    }
}
