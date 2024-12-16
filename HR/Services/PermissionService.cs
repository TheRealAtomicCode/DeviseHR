using Common;
using HR.DTO.Inbound;
using HR.Repository;
using HR.Repository.Interfaces;
using HR.Services.Interfaces;
using HR.Subroutines;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
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

            StringUtils.ValidateNonEmptyStrings([newPermission.PermissionName.Trim()]);

            var permission = newPermission.Adapt<Permission>();
            permission.CompanyId = companyId;
            permission.AddedBy = myId;

            await _permissionRepo.AddPermission(permission);

            await _permissionRepo.SaveChangesAsync();

            return permission.Id;
        }


        public async Task<List<Permission>> GetAllPermissions(int companyId, int? page, int? skip)
        {
            return await _permissionRepo.GetAllPermissionsByCompanyId(companyId, page, skip);
        }


        public async Task EditPermission(JsonPatchDocument<EditPermissionDto> patchDoc, int permissionId, int myId, int companyId)
        {
            var permission = await _permissionRepo.GetPermissionById(permissionId, companyId);

            if (permission == null)
            {
                throw new Exception("Unable to locate permission");
            }

            var toPatch = permission.Adapt<EditPermissionDto>();

            patchDoc.ApplyTo(toPatch);
            toPatch.Adapt(permission);

            permission.UpdatedAt = DateTime.UtcNow;
            permission.UpdatedBy = myId;

            await _permissionRepo.SaveChangesAsync();
        }


    }
}
