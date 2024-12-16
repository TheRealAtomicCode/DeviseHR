using Common;
using HR.DTO.Inbound;
using HR.DTO.outbound;
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

        public PermissionService(IPermissionRepo permissionRepo, IConfiguration configuration)
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


        public async Task<List<SubordinateResponseDto>> GetSubordinatesService(int managerId, int myId, int companyId)
        {
            if (managerId == myId) throw new Exception("Everyone is your subordinate");

            var subordinates = await _permissionRepo.GetSubordinatesByManagerId(managerId, companyId);

            return subordinates;
        }



        public async Task EditSubordinatesService(EditSubordinatesDto editSubordinatesDtos, int myId, int companyId)
        {
            if (editSubordinatesDtos.SubordinatesToBeAdded.Count > 200 || editSubordinatesDtos.SubordinatesToBeRemoved.Count > 200) throw new Exception("Please edit at most 200 emloyees at a time");

            // checking manager is valid
            if (editSubordinatesDtos.ManagerId == myId) throw new Exception("You can not add yourself as a manager since you have admin permissions");
            var manager = await _permissionRepo.GetEmployeeById(editSubordinatesDtos.ManagerId, companyId);
            if (manager == null) throw new Exception("Manager not found");
            if (manager.UserRole != StaticRoles.Manager) throw new Exception("User must be of type manager to have subordinates");

            // checking if the subordinates are managers
            var subordinatesToBeAddedWithoutManagerType = await _permissionRepo.GetNoneManagerEmployeesByIdList(editSubordinatesDtos.SubordinatesToBeAdded, companyId);
            var subordinatesToBeRemovedWithoutManagerType = await _permissionRepo.GetNoneManagerEmployeesByIdList(editSubordinatesDtos.SubordinatesToBeRemoved, companyId);

            if (subordinatesToBeAddedWithoutManagerType.Count > editSubordinatesDtos.SubordinatesToBeAdded.Count ||
                subordinatesToBeRemovedWithoutManagerType.Count > editSubordinatesDtos.SubordinatesToBeRemoved.Count)
            {
                throw new Exception("You can not add users of type manager as subordinates");
            }

            // if statement for the hierarchies to be removed
            if (editSubordinatesDtos.SubordinatesToBeRemoved.Count > 0)
            {
                foreach (var subordinate in subordinatesToBeRemovedWithoutManagerType)
                {
                    if (subordinate.CompanyId != companyId) throw new Exception("Unexpected error, please contact your us");
                    // deleting hierarchy
                    await _permissionRepo.RemoveHierarchy(editSubordinatesDtos.ManagerId, subordinate.Id);
                }
            }

            // if statement for the hierarchies to be added
            if (editSubordinatesDtos.SubordinatesToBeAdded.Count > 0)
            {
                foreach (var subordinate in subordinatesToBeAddedWithoutManagerType)
                {
                    if (subordinate.CompanyId != companyId) throw new Exception("Unexpected error, please contact your manager");
                    // Create a new hierarchy where the managerId is the current manager's Id and the subordinateId is from the subordinatesToBeAddedWithoutManagerType list
                    await _permissionRepo.AddHierarchy(editSubordinatesDtos.ManagerId, subordinate.Id);
                }
            }

            await _permissionRepo.SaveChangesAsync();
        }


    }
}
