using HR.Repository.Interfaces;
using HR.Subroutines;
using HR.UOW.Interfaces;
using Models;

namespace HR.UOW
{
    public class MainUOW : IMainUOW
    {
        private readonly DeviseHrContext _context;

        public IContractRepo ContractRepo { get; }
        public IEmployeeRepo EmployeeRepo { get; }
        public IPermissionRepo PermissionRepo { get; }
        public IHierarchyRepo HierarchyRepo { get; }

        public MainUOW(DeviseHrContext context, IContractRepo contractRepo, IEmployeeRepo employeeRepo, IPermissionRepo permissionRepo, IHierarchyRepo hierarchyRepo)
        {
            _context = context;
            ContractRepo = contractRepo;
            EmployeeRepo = employeeRepo;
            PermissionRepo = permissionRepo;
            HierarchyRepo = hierarchyRepo;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                SqlExceptionHandler.ExceptionHandler(ex);
            }
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

    }

}
