using HR.DTO.Inbound;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using System.ComponentModel.Design;

namespace HR.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<int> CreatePermissionService(PermissionData newPermission, int myId, int companyId);
        Task<List<Permission>> GetAllPermissions(int companyId, int? page, int? skip);
        //Task<List<Permission>> GetAllPermissions(int permissionId, int companyId, int page, int skip);
    }
}
