using Common;
using HR.DTO;
using HR.Repository;
using HR.Subroutines;
using HR.UOW;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Data;

namespace HR.Services
{
    public class PermissionService 
    {

        private readonly MainUOW _mainUOW;
        private readonly IConfiguration _configuration;

        public PermissionService(MainUOW mainUOW, IConfiguration configuration)
        {
            _mainUOW = mainUOW;
            _configuration = configuration;
        }


        public async Task<int> CreatePermissionService(AddPermissionRequest newPermission, int myId, int companyId)
        {

            StringUtils.ValidateNonEmptyStrings([newPermission.PermissionName.Trim()]);

            var permission = newPermission.Adapt<Permission>();
            permission.CompanyId = companyId;
            permission.AddedBy = myId;

            await _mainUOW.PermissionRepo.AddPermission(permission);

            await _mainUOW.SaveChangesAsync();

            return permission.Id;
        }


        public async Task<List<Permission>> GetAllPermissions(int companyId, int? page, int? skip)
        {
            return await _mainUOW.PermissionRepo.GetAllPermissionsByCompanyId(companyId, page, skip);
        }


        public async Task EditPermission(JsonPatchDocument<EditPermissionRequest> patchDoc, int permissionId, int myId, int companyId)
        {
            var permission = await _mainUOW.PermissionRepo.GetPermissionById(permissionId, companyId);

            if (permission == null)
            {
                throw new Exception("Unable to locate permission");
            }

            var toPatch = permission.Adapt<EditPermissionRequest>();

            patchDoc.ApplyTo(toPatch);
            toPatch.Adapt(permission);

            permission.UpdatedAt = DateTime.UtcNow;
            permission.UpdatedBy = myId;

            await _mainUOW.SaveChangesAsync();
        }


        public async Task<List<SubordinateResponseDto>> GetSubordinatesService(int managerId, int myId, int companyId)
        {
            if (managerId == myId) throw new Exception("Everyone is your subordinate");

            var subordinates = await _mainUOW.HierarchyRepo.GetSubordinatesByManagerId(managerId, companyId);

            return subordinates;
        }



        public async Task EditSubordinatesService(EditSubordinatesRequest editSubordinatesDtos, int myId, int companyId)
        {
            if (editSubordinatesDtos.SubordinatesToBeAdded.Count > 200 || editSubordinatesDtos.SubordinatesToBeRemoved.Count > 200) throw new Exception("Please edit at most 200 emloyees at a time");

            // checking manager is valid
            if (editSubordinatesDtos.ManagerId == myId) throw new Exception("You can not add yourself as a manager since you have admin permissions");
            var manager = await _mainUOW.EmployeeRepo.GetEmployeeById(editSubordinatesDtos.ManagerId, companyId);
            if (manager == null) throw new Exception("Manager not found");
            if (manager.UserRole != StaticRoles.Manager) throw new Exception("User must be of type manager to have subordinates");

            // checking if the subordinates are managers
            var subordinatesToBeAddedWithoutManagerType = await _mainUOW.HierarchyRepo.GetNoneManagerEmployeesByIdList(editSubordinatesDtos.SubordinatesToBeAdded, companyId);
            var subordinatesToBeRemovedWithoutManagerType = await _mainUOW.HierarchyRepo.GetNoneManagerEmployeesByIdList(editSubordinatesDtos.SubordinatesToBeRemoved, companyId);

            if (subordinatesToBeAddedWithoutManagerType.Count < editSubordinatesDtos.SubordinatesToBeAdded.Count ||
                subordinatesToBeRemovedWithoutManagerType.Count < editSubordinatesDtos.SubordinatesToBeRemoved.Count)
            {
                throw new Exception("You can not add users of type manager as subordinates");
            }

            // if statement for the hierarchies to be added
            if (editSubordinatesDtos.SubordinatesToBeAdded.Count > 0)
            {
                foreach (var subordinate in subordinatesToBeAddedWithoutManagerType)
                {
                    if (subordinate.CompanyId != companyId) throw new Exception("Unexpected error, please contact your manager");
                    // Create a new hierarchy where the managerId is the current manager's Id and the subordinateId is from the subordinatesToBeAddedWithoutManagerType list
                    await _mainUOW.HierarchyRepo.AddHierarchy(editSubordinatesDtos.ManagerId, subordinate.Id);
                }
            }

            // if statement for the hierarchies to be removed
            if (editSubordinatesDtos.SubordinatesToBeRemoved.Count > 0)
            {
                foreach (var subordinate in subordinatesToBeRemovedWithoutManagerType)
                {
                    if (subordinate.CompanyId != companyId) throw new Exception("Unexpected error, please contact your us");
                    // deleting hierarchy
                    await _mainUOW.HierarchyRepo.RemoveHierarchy(editSubordinatesDtos.ManagerId, subordinate.Id);
                }
            }

            await _mainUOW.SaveChangesAsync();
        }


    }
}
