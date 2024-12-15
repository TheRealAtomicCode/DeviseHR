using Common;
using HR.DTO.Inbound;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.Subroutines;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Data;

namespace HR.Services
{
    public class PermissionService : IPermissionService
    {

        private readonly IPermissionRepo _permissionRepo;
        private readonly IConfiguration _configuration;

        public PermissionService(IPermissionRepo permissionRepo, IHierarchyRepo hierarchyRepo, IConfiguration configuration)
        {
            _permissionRepo = permissionRepo;
            _configuration = configuration;
        }


        public async Task<Permission> CreatePermissionService(PermissionData newPermission, int myId, int companyId)
        {

            StringUtils.ValidateNonEmptyStrings(newPermission.Name.Trim());

            //Role role = AdminRoleRepository.InsertRole(newRole, myId, companyId, db);

            //try
            //{
            //    await db.SaveChangesAsync();
            //}
            //catch (DbUpdateException ex)
            //{
            //    SqlExceptionHandler.RoleSqlExceptionHandler(ex);
            //}

            //return role;

            throw new NotImplementedException();
        }
    }
}
