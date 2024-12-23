using HR.Repository.Interfaces;
using HR.Subroutines;
using HR.UOW.Interfaces;
using Models;

namespace HR.UOW
{
    public class CEP_UOW : ICEP_UOW
    {
        private readonly DeviseHrContext _context;

        public IContractRepo ContractRepo { get; }
        public IEmployeeRepo EmployeeRepo { get; }
        public IPermissionRepo PermissionRepo { get; }

        public CEP_UOW(DeviseHrContext context, IContractRepo contractRepo, IEmployeeRepo employeeRepo, IPermissionRepo permissionRepo)
        {
            _context = context;
            ContractRepo = contractRepo;
            EmployeeRepo = employeeRepo;
            PermissionRepo = permissionRepo;
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
