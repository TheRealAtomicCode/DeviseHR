using HR.Repository;
using HR.Subroutines;
using Models;

namespace HR.UOW
{
    public class MainUOW
    {
        private readonly DeviseHrContext _context;

        public ContractRepo ContractRepo { get; }
        public EmployeeRepo EmployeeRepo { get; }
        public PermissionRepo PermissionRepo { get; }
        public HierarchyRepo HierarchyRepo { get; }
        public AbsenceRepo AbsenceRepo { get; }
        public WorkingPatternRepo WorkingPatternRepo { get; }

        public MainUOW(DeviseHrContext context, ContractRepo contractRepo, EmployeeRepo employeeRepo, PermissionRepo permissionRepo, HierarchyRepo hierarchyRepo, AbsenceRepo absenceRepo, WorkingPatternRepo workingPatternRepo)
        {
            _context = context;
            ContractRepo = contractRepo;
            EmployeeRepo = employeeRepo;
            PermissionRepo = permissionRepo;
            HierarchyRepo = hierarchyRepo;
            AbsenceRepo = absenceRepo;
            WorkingPatternRepo = workingPatternRepo;
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
