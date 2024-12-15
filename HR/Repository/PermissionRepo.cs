using HR.DTO.Inbound;
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
