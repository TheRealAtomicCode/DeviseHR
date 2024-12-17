using HR.DTO;
using HR.DTO.Inbound;
using HR.DTO.outbound;
using HR.Repository.Interfaces;
using HR.Subroutines;
using Microsoft.EntityFrameworkCore;
using Models;

namespace HR.Repository
{
    public class PermissionRepo : IPermissionRepo
    {
        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public PermissionRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }


        public async Task AddPermission(Permission newPermission)
        {
            await _context.Permissions.AddAsync(newPermission);
        }

        public async Task AddHierarchy(Hierarchy hierarchy)
        {
            await _context.AddAsync(hierarchy);
        }

        public async Task<List<Permission>> GetAllPermissionsByCompanyId(int companyId, int? page, int? skip)
        {
            var query = _context.Permissions.AsQueryable();
            query = query.Where(p => p.CompanyId == companyId);

            if (page.HasValue && skip.HasValue)
            {
                int skipCount = Math.Abs((page.Value - 1) * skip.Value);
                int takeCount = Math.Abs(skip.Value);

                query = query.Skip(skipCount).Take(takeCount);
            }

            return await query.ToListAsync();
        }

        public async Task<Permission?> GetPermissionById(int id, int companyId)
        {
            return await _context.Permissions.Where(p => p.Id == id && p.CompanyId == companyId).FirstOrDefaultAsync();
        }

        public async Task<List<SubordinateResponseDto>> GetSubordinatesByManagerId(int managerId, int companyId)
        {
            var userInfos = await 
               (from u in _context.Employees
                join h in _context.Hierarchies on u.Id equals h.SubordinateId into subordinates
                from s in subordinates.DefaultIfEmpty()
                where u.CompanyId == companyId && u.Id != managerId
                select new SubordinateResponseDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserRole = u.UserRole,
                    Id = u.Id,
                    //IsSubordinate = (s != null && s.ManagerId == managerId)
                }
            ).ToListAsync();

            var xxx = await (
                from e in _context.Employees
                join h in _context.Hierarchies
                on e.Id equals h.SubordinateId
                where h.ManagerId == managerId && e.CompanyId == companyId
                select new SubordinateResponseDto
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Email = e.Email,
                    UserRole = e.UserRole,
                    Id = e.Id
                }).ToListAsync();

            return xxx;
        }

        public async Task<List<NonManagerUserDto>> GetNoneManagerEmployeesByIdList(List<int> employeeIds, int companyId)
        {
            List<NonManagerUserDto> adminsAndEmployees = await _context.Employees
                .Where(u => employeeIds.Contains(u.Id) && u.UserRole != StaticRoles.Manager && u.CompanyId == companyId)
                .Select(u => new NonManagerUserDto
                {
                    Id = u.Id,
                    UserRole = u.UserRole,
                    CompanyId = u.CompanyId
                })
                .ToListAsync();

            return adminsAndEmployees;
        }

        public async Task RemoveHierarchy(int managerId, int subordinateId)
        {
            await _context.Hierarchies.Where(h => h.ManagerId == managerId && h.SubordinateId == subordinateId).ExecuteDeleteAsync();
        }

        public async Task AddHierarchy(int managerId, int subordinateId)
        {
            await _context.Hierarchies.AddAsync(new Hierarchy
            {
                ManagerId = managerId,
                SubordinateId = subordinateId
            });
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

        // copied

        public async Task<Employee?> GetEmployeeById(int id, int companyId)
        {
            return await _context.Employees
                .Include(u => u.Company)
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(emp => emp.Id == id && emp.CompanyId == companyId);
        }

    }
}
