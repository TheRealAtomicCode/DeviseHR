using Microsoft.EntityFrameworkCore;

namespace HR.Subroutines
{

    public class SqlExceptionHandler
    {
        public static void ExceptionHandler(Exception ex)
        {
            if (ex.InnerException == null || ex.InnerException.Message == null) throw new Exception("An unexpected error occurred");

            string errorMessage = ex.InnerException.Message;
            string constraintName = ExtractConstraintName(errorMessage);

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


        private static string ExtractConstraintName(string errorMessage)
        {
            // Look for the constraint violation message and extract the constraint name
            string pattern = @"violates (?:foreign key|check) constraint ""(.+?)""";
            var match = System.Text.RegularExpressions.Regex.Match(errorMessage, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value; // Returns the constraint name (e.g., "half_days_for_leaves_in_days")
            }

            return null; // No constraint found
        }

    }

}


