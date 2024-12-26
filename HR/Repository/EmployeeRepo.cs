using Microsoft.EntityFrameworkCore;
using Models;
using HR.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;
using HR.Subroutines;
using Mapster;
using HR.DTO;

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


        public async Task<List<FoundEmployee>> GetAllEmployeesByName(string? searchTerm, int? page, int? skip, int companyId, int? myId)
        {
            IQueryable<Employee> query = _context.Employees
                .Where(e => e.CompanyId == companyId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => (e.FirstName + " " + e.LastName).Contains(searchTerm) || e.Email.Contains(searchTerm));
            }

            if (myId != null)
            {
                query = query.Where(e => _context.Hierarchies.Any(h => h.ManagerId == myId));
            }

            if (page != null && skip != null)
            {
                int skipCount = Math.Abs((int)((page - 1) * skip));
                int takeCount = Math.Abs((int)skip);

                query = query.Skip(skipCount).Take(takeCount);
            }

            var employees = await query   
                .Select(e => new FoundEmployee
                {
                    Id = e.Id,
                    FullName = $"{e.FirstName} {e.LastName}",
                    Title = e.Title,
                    Email = e.Email,
                    UserRole = e.UserRole,
                    AnnualLeaveStartDate = e.AnnualLeaveStartDate                    
                })
                .ToListAsync();

            return employees;
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
