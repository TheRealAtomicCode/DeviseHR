using Common;
using HR.DTO.Inbound;
using HR.Repository;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.Subroutines;
using Mapster;
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


        public async Task<int> CreatePermissionService(PermissionData newPermission, int myId, int companyId)
        {

            StringUtils.ValidateNonEmptyStrings(newPermission.Name.Trim());

            var permission = newPermission.Adapt<Permission>();
            permission.CompanyId = companyId;
            permission.AddedBy = myId;

            await _permissionRepo.AddPermission(permission);

            await _permissionRepo.SaveChangesAsync();

            return permission.Id;
        }
    }
}
