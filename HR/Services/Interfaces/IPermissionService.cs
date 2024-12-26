
using HR.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.ComponentModel.Design;

namespace HR.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<int> CreatePermissionService(AddPermissionRequest newPermission, int myId, int companyId);
        Task<List<Permission>> GetAllPermissions(int companyId, int? page, int? skip);
        Task EditPermission(JsonPatchDocument<EditPermissionRequest> patchDoc, int permissionId, int myId, int companyId);
        Task<List<SubordinateResponseDto>> GetSubordinatesService(int managerId, int myId, int companyId);
        Task EditSubordinatesService(EditSubordinatesRequest editSubordinatesDto, int myId, int companyId);

    }
}
