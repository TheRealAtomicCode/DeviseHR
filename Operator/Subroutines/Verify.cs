using Models;

namespace OP.Subroutines
{
    public class Verify
    {

        public static void OperatorAccess(Operator op, string loginAttempLimit)
        {
            if (op.IsTerminated) throw new Exception("Your Permissions have been Revoked.");

            if (op.LastLoginTime == null && op.IsVerified == false) throw new Exception("Please sign into your account with the registration email that was sent to you. Please make sure to check the junk and spam folders. If you did not receive it, please contact your manager.");

            int loginAttemptsAllowed = int.Parse(loginAttempLimit);

            if (op.LoginAttempt > loginAttemptsAllowed) throw new Exception("You have attempted to login multiple times unsuccessfully. Please contact your manager to regain access to your account.");
        }



    }
}

