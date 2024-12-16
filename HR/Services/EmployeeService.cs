using Common;
using HR.DTO.Inbound;
using HR.DTO.outbound;
using HR.Repository;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Models;


namespace HR.Services
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

            if (myRole <= StaticRoles.Manager && newEmployee.UserRole >= StaticRoles.Manager) throw new Exception("You can not add managers or admins if you have manager permissions");
            if (myRole <= StaticRoles.Manager && newEmployee.PermissionId != null) throw new Exception("You can not add employees with special permissions");

            await _employeeRepo.AddEmployee(employee);

            if (newEmployee.RegisterUser == true)
            {
                string otp = StringUtils.GenerateSixDigitString();
                // send verification code
                employee.VerificationCode = otp;
            }

            await _employeeRepo.SaveChangesAsync();

            if (myRole <= StaticRoles.Manager)
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


        public async Task<EmployeeDto> GetEmployee(int employeeId, int myId, int companyId, int myRole)
        {
            EmployeeDto employee = await _employeeRepo.GetEmployeeDtoById(employeeId, companyId);

            if (myId != employee.Id && myRole <= StaticRoles.StaffMember) throw new Exception("Insufficient permissions");

            if (myId != employee.Id && myRole == StaticRoles.Manager)
            {
                if (!employee.Managers.Any(m => m.ManagerId == myId))
                {
                    throw new Exception("You are not authorized to access this employee's details.");
                }
            }

            return employee;
        }


        public async Task<List<FoundEmployee>> GetAllEmployees(string? searchTerm, int? page, int? skip, int myId, int companyId, int myRole, bool enableShowEmployees)
        {
            int? myIdWhenSearching = null;

            if (myRole <= StaticRoles.StaffMember && enableShowEmployees == false) return new List<FoundEmployee>();

            if (myRole == StaticRoles.Manager && enableShowEmployees == false) myIdWhenSearching = myId;

            List<FoundEmployee> foundEmployees = await _employeeRepo.GetAllEmployeesByName(searchTerm, page, skip, companyId, myIdWhenSearching);

            return foundEmployees;
        }

        public async Task EditEmployee(JsonPatchDocument<EditEmployeeDto> patchDoc, int employeeId, int myId, int companyId)
        {
            var employee = await _employeeRepo.GetEmployeeById(employeeId, companyId);

            if (employee == null)
            {
                throw new Exception("Unable to locate employee");
            }

            var toPatch = employee.Adapt<EditEmployeeDto>();

            patchDoc.ApplyTo(toPatch);
            toPatch.Adapt(employee);

            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedByUser = myId;

            await _employeeRepo.SaveChangesAsync();
        }

      



    }
}
