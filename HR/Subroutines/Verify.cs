using Models;

namespace HR.Subroutines
{
    public class Verify
    {

        public static void EmployeeAccess(Employee emp, string loginAttempLimit)
        {
            if (emp.IsTerminated) throw new Exception("Your Permissions have been Revoked.");

            if (emp.LastLoginTime == null && emp.IsVerified == false) throw new Exception("Please sign into your account with the registration email that was sent to you. Please make sure to check the junk and spam folders. If you did not receive it, please contact your manager.");

            int loginAttemptsAllowed = int.Parse(loginAttempLimit);

            if (emp.LoginAttempt > loginAttemptsAllowed) throw new Exception("You have attempted to login multiple times unsuccessfully. Please contact your manager to regain access to your account.");
        }



    }
}

