using Common;
using HR.DTO.Inbound;
using HR.Repository;
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
        private readonly IHierarchyRepo _hierarchyRepo;
        private readonly IConfiguration _configuration;

        public EmployeeService(IEmployeeRepo employeeRepo, IHierarchyRepo hierarchyRepo, IConfiguration configuration)
        {
            _employeeRepo = employeeRepo;
            _hierarchyRepo = hierarchyRepo;
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
            if (myRole >= 3 && newEmployee.PermissionId != null) throw new Exception("You can not provide employees permissions");

            await _employeeRepo.AddEmployee(employee);

            if (newEmployee.RegisterUser == true)
            {
                string otp = StringUtils.GenerateSixDigitString();
                // send verification code
                employee.VerificationCode = otp;
            }
            
            await _employeeRepo.SaveChangesAsync();

            if (myRole >= 3)
            {
                Hierarchy hierarchy = new Hierarchy
                {
                    ManagerId = myId,
                    SubordinateId = employee.Id
                };

                await _hierarchyRepo.AddHierarchy(hierarchy);
            }

            await _hierarchyRepo.SaveChangesAsync();

            return employee.Id;
        }


        public async Task GetEmployee(int employeeId, int myId, int companyId, int myRole)
        {
            Employee? employee = await _employeeRepo.GetEmployeeById(employeeId, companyId);

            if (!employee) throw new Exception("Unable to locate");

            if (myId != employee.Id && (myRole == RoleIds.EmployeeId || myRole == RoleIds.VisitorId)) throw new Exception("Access denined");
            if (myId != employeeId && (myRole != RoleIds.ManagerId)) throw new Exception("Access denined");

            if (employee == null) throw new Exception("Employee not found");

            


            if (myRole >= 3)
            {
                Hierarchy hierarchy = new Hierarchy
                {
                    ManagerId = myId,
                    SubordinateId = employee.Id
                };

                await _hierarchyRepo.AddHierarchy(hierarchy);
            }

            await _hierarchyRepo.SaveChangesAsync();

            return employee.Id;
        }


    }
}
