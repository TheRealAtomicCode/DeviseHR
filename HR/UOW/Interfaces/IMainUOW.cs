using HR.Repository.Interfaces;

namespace HR.UOW.Interfaces
{
    public interface IMainUOW
    {
        IContractRepo ContractRepo { get; }
        IEmployeeRepo EmployeeRepo { get; }
        IPermissionRepo PermissionRepo { get; }
        IHierarchyRepo HierarchyRepo { get; }
        Task SaveChangesAsync();
        Task DisposeAsync();
    }
 
}
