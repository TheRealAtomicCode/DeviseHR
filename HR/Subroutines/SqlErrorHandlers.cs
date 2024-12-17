using Microsoft.EntityFrameworkCore;

namespace HR.Subroutines
{

    public class SqlExceptionHandler
    {
        public static void ExceptionHandler(Exception ex)
        {
            if (ex.InnerException == null) throw new Exception("An unexpected error occurred");

            string errorMessage = ex.InnerException.Message;

            if (errorMessage.Contains("employee_email_key")) throw new Exception("Email already exists");

            if (errorMessage.Contains("employee_permission_id_fkey")) throw new Exception("Permission does not exist");

            if (errorMessage.Contains("permission_id_required_case_manager")) throw new Exception("Managers must be provided with a specific permission");

            if (errorMessage.Contains("permission_id_null_if_not_manager")) throw new Exception("Only managers can be provided with a permission");

            if (errorMessage.Contains("uq_hierarchies")) throw new Exception("The provided employees have already been added as subordinates to that manager");

            throw new Exception("An unexpected error occurred");
        }

    }

}


