using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using HR.DTO.Inbound;
using HR.Subroutines;
using HR.DTO.outbound;
using Mapster;

namespace HR.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {


        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public EmployeeRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }

        public async Task<Employee?> GetEmployeeByEmailOrDefault(string email)
        {
            return await _context.Employees
                .Include(u => u.Company)
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(emp => emp.Email == email.Trim().ToLower());
        }

        public async Task<Employee?> GetEmployeeById(int id, int companyId)
        {
            return await _context.Employees
                .Include(u => u.Company)
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(emp => emp.Id == id && emp.CompanyId == companyId);
        }


        public async Task<EmployeeDto> GetEmployeeDtoById(int employeeId, int companyId)
        {

            var employeeDto = await (from e in _context.Employees
                                     where e.Id == employeeId && e.CompanyId == companyId
                                     select new EmployeeDto
                                     {
                                         Id = e.Id,
                                         FirstName = e.FirstName,
                                         LastName = e.LastName,
                                         Title = e.Title,
                                         Email = e.Email,
                                         DateOfBirth = e.DateOfBirth,
                                         AnnualLeaveStartDate = e.AnnualLeaveStartDate,
                                         ProfilePicture = e.ProfilePicture,
                                         IsTerminated = e.IsTerminated,
                                         IsVerified = e.IsVerified,
                                         CreatedAt = e.CreatedAt,
                                         NiNo = e.NiNo,
                                         DriversLicenceNumber = e.DriversLicenceNumber,
                                         DriversLicenceExpirationDate = e.DriversLicenceExpirationDate,
                                         PassportNumber = e.PassportNumber,
                                         PassportExpirationDate = e.PassportExpirationDate,
                                         UserRole = e.UserRole,
                                         PermissionId = e.PermissionId,
                                         Managers = new List<ManagerDto>()
                                     }).FirstOrDefaultAsync();

            if (employeeDto == null) throw new Exception("Employee not found");

            List<ManagerDto> managers = await (from h in _context.Hierarchies
                                               join m in _context.Employees on h.ManagerId equals m.Id
                                               where h.SubordinateId == employeeDto.Id
                                               select new ManagerDto
                                               {
                                                   ManagerId = m.Id,
                                                   FullName = m.FirstName + " " + m.LastName,
                                                   Title = m.Title,
                                                   Email = m.Email
                                               }).ToListAsync();

            employeeDto.Managers = managers;

            return employeeDto;
        }


        public async Task<List<Employee>> GetAllEmployees(string email)
        {
            return await _context.Employees.ToListAsync();
        }

        

        public async Task AddEmployee(Employee newEmployee)
        {
            await _context.Employees.AddAsync(newEmployee);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                SqlExceptionHandler.ExceptionHandler(ex);
            }
       
        }

       
    }
}
