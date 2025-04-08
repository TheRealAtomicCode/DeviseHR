using Microsoft.EntityFrameworkCore;

namespace HR.Subroutines
{

    public class SqlExceptionHandler
    {
        public static void ExceptionHandler(Exception ex)
        {
            if (ex.InnerException == null || ex.InnerException.Message == null) throw new Exception("An unexpected error occurred");

            string errorMessage = ex.InnerException.Message;
            string? constraintName = ExtractConstraintName(errorMessage);

            switch (constraintName)
            {
                case "employee_email_key":
                    throw new Exception("Email already exists");

                case "employee_permission_id_fkey":
                    throw new Exception("Permission does not exist");

                case "permission_id_required_case_manager":
                    throw new Exception("Managers must be provided with a specific permission");

                case "permission_id_null_if_not_manager":
                    throw new Exception("Only managers can be provided with a permission");

                case "uq_hierarchies":
                    throw new Exception("The provided employees have already been added as subordinates to that manager");

                case "half_days_for_leaves_in_days":
                    throw new Exception("You can not add half days to contracts that are in hours");

                case "fk_absence_type":
                    throw new Exception("Absence type required");

                default:
                    throw new Exception("An unexpected error occurred");
            }
        }



        public static string? ExtractConstraintName(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return null;

            int startIndex = errorMessage.IndexOf('\"');
            if (startIndex == -1)
                return null;
            
            int endIndex = errorMessage.IndexOf('\"', startIndex + 1);
            if (endIndex == -1)
                return null;

            return errorMessage.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

    }

}


