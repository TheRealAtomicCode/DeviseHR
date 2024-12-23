using HR.Repository.Interfaces;

namespace HR.UOW.Interfaces
{
    public interface ICEP_UOW
    {
        IContractRepo ContractRepo { get; }
        IEmployeeRepo EmployeeRepo { get; }
        IPermissionRepo PermissionRepo { get; }
        Task SaveChangesAsync();
        Task DisposeAsync();
    }
 
}
