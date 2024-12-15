using HR.DTO.Inbound;
using HR.Subroutines;
using Microsoft.EntityFrameworkCore;
using Models;

namespace HR.Repository.Interfaces
{
    public interface IPermissionRepo
    {
        Task AddPermission(Permission newPermission);
        Task SaveChangesAsync();
    }

    
    
}
