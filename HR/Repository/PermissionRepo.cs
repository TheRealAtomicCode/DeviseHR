using HR.DTO;
using HR.Subroutines;
using Microsoft.EntityFrameworkCore;
using Models;

namespace HR.Repository
{
    public class PermissionRepo
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
