using HR.Repository;
using Models;
using System.Security.Claims;

namespace HR.Subroutines
{
    public class GenerateClaims
    {

        public static List<Claim> GetEmployeeJwtClaims(Employee employee)
        {
            var claims = new List<Claim>();

            if (employee.UserRole == StaticRoles.Sudo || employee.UserRole == StaticRoles.Admin)
            {
                claims = new List<Claim>
                {
                    new Claim("id", employee.Id.ToString()),
                    // companyId and companySettings
                    new Claim("companyId", employee.Company.Id.ToString()),
                    new Claim("enableSemiPersonalInformation", "true"),
                    new Claim("enableShowEmployees", "true"),
                    new Claim("enableToil", "true"),
                    new Claim("enableOvertime", "true"),
                    new Claim("enableAbsenceConflictsOutsideDepartments", "true"),
                    new Claim("enableCarryover", "true"),
                    new Claim("enableSelfCancelLeaveRequests", "true"),
                    new Claim("enableEditMyInformation", "true"),
                    new Claim("enableAcceptDeclineShifts", "true"),
                    new Claim("enableTakeoverShifts", "true"),
                    new Claim("enableBraudcastShiftSwap", "true"),
                    new Claim("enableTwoStageApproval", "true"),
                    // user roles and permissions
                    new Claim("userRole", employee.UserRole.ToString()),
                    new Claim("permissionId", "0"),
                    new Claim("enableAddEmployees", "true"),
                    new Claim("enableAddLateness", "true"),
                    new Claim("enableAddManditoryLeave", "true"),
                    new Claim("enableApproveAbsence", "true"),
                    new Claim("enableCreatePattern", "true"),
                    new Claim("enableCreateRotas", "true"),
                    new Claim("enableDeleteEmployee", "true"),
                    new Claim("enableTerminateEmployees", "true"),
                    new Claim("enableViewEmployeeNotifications", "true"),
                    new Claim("enableViewEmployeePayroll", "true"),
                    new Claim("enableViewEmployeeSensitiveInformation", "true"),
                    new Claim("annualLeaveStartDate", employee.Company.AnnualLeaveStartDate.ToString()!)
                };
            }
            else if (employee.UserRole == StaticRoles.Manager && employee.Permission != null)
            {
                claims = new List<Claim>
                {
                    new Claim("id", employee.Id.ToString()),
                    // companyId and companySettings
                    new Claim("companyId", employee.Company.Id.ToString()),
                    new Claim("enableSemiPersonalInformation", employee.Company.EnableSemiPersonalInformation ? "true" : "false"),
                    new Claim("enableShowEmployees", employee.Company.EnableShowEmployees ? "true" : "false"),
                    new Claim("enableToil", employee.Company.EnableToil ? "true" : "false"),
                    new Claim("enableOvertime", employee.Company.EnableOvertime ? "true" : "false"),
                    new Claim("enableAbsenceConflictsOutsideDepartments", employee.Company.EnableAbsenceConflictsOutsideDepartments ? "true" : "false"),
                    new Claim("enableCarryover", employee.Company.EnableCarryover ? "true" : "false"),
                    new Claim("enableSelfCancelLeaveRequests", employee.Company.EnableSelfCancelLeaveRequests ? "true" : "false"),
                    new Claim("enableEditMyInformation", employee.Company.EnableEditMyInformation ? "true" : "false"),
                    new Claim("enableAcceptDeclineShifts", employee.Company.EnableAcceptDeclineShifts ? "true" : "false"),
                    new Claim("enableTakeoverShifts", employee.Company.EnableTakeoverShift ? "true" : "false"),
                    new Claim("enableBroadcastShiftSwap", employee.Company.EnableBroadcastShiftSwap ? "true" : "false"),
                    new Claim("enableTwoStageApproval", employee.Company.EnableRequireTwoStageApproval ? "true" : "false"),
                    // user roles and permissions
                    new Claim("userRole", employee.UserRole.ToString()),
                    new Claim("permissionId", employee.PermissionId != null ? employee.PermissionId.ToString()! : "0"),
                    new Claim("enableAddEmployees", employee.Permission.EnableAddEmployees ? "true" : "false"),
                    new Claim("enableAddLateness", employee.Permission.EnableAddLateness ? "true" : "false"),
                    new Claim("enableAddManditoryLeave", employee.Permission.EnableAddManditoryLeave ? "true" : "false"),
                    new Claim("enableApproveAbsence", employee.Permission.EnableApproveAbsence ? "true" : "false"),
                    new Claim("enableCreatePattern", employee.Permission.EnableCreatePattern ? "true" : "false"),
                    new Claim("enableCreateRotas", employee.Permission.EnableCreateRotas ? "true" : "false"),
                    new Claim("enableDeleteEmployee", employee.Permission.EnableDeleteEmployee ? "true" : "false"),
                    new Claim("enableTerminateEmployees", employee.Permission.EnableTerminateEmployees ? "true" : "false"),
                    new Claim("enableViewEmployeeNotifications", employee.Permission.EnableViewEmployeeNotifications ? "true" : "false"),
                    new Claim("enableViewEmployeePayroll", employee.Permission.EnableViewEmployeePayroll ? "true" : "false"),
                    new Claim("enableViewEmployeeSensitiveInformation", employee.Permission.EnableViewEmployeeSensitiveInformation ? "true" : "false"),
                    new Claim("annualLeaveStartDate", employee.Company.AnnualLeaveStartDate.ToString()!)
                };
            }
            else if (employee.UserRole == StaticRoles.StaffMember || employee.UserRole == StaticRoles.Visitor)
            {
                claims = new List<Claim>
                {
                    new Claim("id", employee.Id.ToString()),
                    new Claim("companyId", employee.Company.Id.ToString()),
                    new Claim("userRole", employee.UserRole.ToString()),
                    new Claim("annualLeaveStartDate", employee.Company.AnnualLeaveStartDate.ToString()!)
                };
            }

            return claims;
        }


        public static List<Claim> GetEmployeeRefreshTokenClaims(Employee employee)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", employee.Id.ToString()),
                new Claim("userRole", employee.UserRole.ToString()),
                new Claim("companyId", employee.CompanyId.ToString()),
            };

            return claims;
        }

    }
}
