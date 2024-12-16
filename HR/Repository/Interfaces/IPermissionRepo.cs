using HR.DTO.Inbound;
using HR.Subroutines;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Models;
using System.ComponentModel.Design;

namespace HR.Repository.Interfaces
{
    public interface IPermissionRepo
    {
        Task AddPermission(Permission newPermission);
        Task<List<Permission>> GetAllPermissionsByCompanyId(int companyId, int? page, int? skip);
        Task SaveChangesAsync();
    }

    
    
}
