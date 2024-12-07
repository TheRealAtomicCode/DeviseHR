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

            throw new Exception("An unexpected error occurred");
        }

    }

}


