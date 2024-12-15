using HR.DTO.Inbound;
using Models;

namespace HR.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<Permission> CreatePermissionService(PermissionData newPermission, int myId, int companyId);
    }
}
