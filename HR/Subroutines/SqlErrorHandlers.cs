using Microsoft.EntityFrameworkCore;

namespace HR.Subroutines
{

    public class SqlExceptionHandler
    {
        public static void ExceptionHandler(DbUpdateException ex, string contains)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains(contains))
            {
                throw new Exception("Email already exists");
            }

            throw new Exception("An unexpected Error occured");

            throw ex;
        }

    }

}


