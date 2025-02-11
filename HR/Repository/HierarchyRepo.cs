using HR.DTO;
using Microsoft.EntityFrameworkCore;
using Models;

namespace HR.Repository
{
    public class HierarchyRepo
    {
        private readonly DeviseHrContext _context;
        private readonly IConfiguration _configuration;

        public HierarchyRepo(DeviseHrContext dbContext, IConfiguration configuration)
        {
            _context = dbContext;
            _configuration = configuration;
        }


        public async Task AddHierarchy(int managerId, int subordinateId)
        {
            await _context.Hierarchies.AddAsync(new Hierarchy
            {
                ManagerId = managerId,
                SubordinateId = subordinateId
            });
        }

        //public async Task AddHierarchy(Hierarchy hierarchy)
        //{
        //    await _context.AddAsync(hierarchy);
        //}

        public async Task RemoveHierarchy(int managerId, int subordinateId)
        {
            await _context.Hierarchies.Where(h => h.ManagerId == managerId && h.SubordinateId == subordinateId).ExecuteDeleteAsync();
        }


        public async Task<bool> HasManager(int subordinateId)
        {
            var hierarchy = await _context.Hierarchies.FirstOrDefaultAsync(s => s.SubordinateId == subordinateId);

            bool hasManager = hierarchy != null;

            return hasManager;
        }

        public async Task<bool> IsRelated(int managerId, int subordinateId)
        {
            var hierarchy = await _context.Hierarchies.FirstOrDefaultAsync(s => s.ManagerId == managerId && s.SubordinateId == subordinateId);

            bool isRelated = hierarchy != null;

            return isRelated;
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


        public async Task<List<SubordinateResponseDto>> GetSubordinatesByManagerId(int managerId, int companyId)
        {
            var subordinates = await (
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

            return subordinates;
        }


        // sub repos
        public async Task<int> ValidateRequestOrAddAbsence(int myId, int userRole, int userId)
        {

            // -1 error
            // 0 request
            // 1 add

            if (userId == myId)
            {
                if (userRole >= StaticRoles.Admin)
                {
                    // check if i have a manager
                    bool hasManager = await HasManager(myId);

                    if (hasManager)
                    {
                        // request
                        return 0;
                    }
                    else
                    {
                        // add
                        return 1;
                    }
                }
                else
                {
                    // request
                    return 0;
                }


            }
            else
            {
                if (userRole >= StaticRoles.Admin)
                {
                    // add
                    return 1;
                }
                else if (userRole == StaticRoles.Manager)
                {
                    bool isSubordinate = await IsRelated(myId, userId);

                    if (isSubordinate)
                    {
                        // add
                        return 1;
                    }
                    else
                    {
                        // error
                        return -1;
                    }
                }
                else
                {
                    // error
                    return -1;
                }

            }

        }





    }
}
