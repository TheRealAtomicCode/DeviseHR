using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{

    public class StringUtils
    {


        public static string GenerateSixDigitString()
        {
            Random random = new Random();
            int code = random.Next(1000000, 10000000);
            return code.ToString();
        }




        public static void ValidateNonEmptyStrings(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(arr[i]))
                {
                    throw new Exception($"String at index {i} is empty");
                }
            }
        }

        public static void ValidateEmail(string email)
        {
            string regexPattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            bool passed = Regex.IsMatch(email, regexPattern);
            if (!passed)
            {
                throw new Exception("Please provide a valid email address.");
            }
        }

        public static void ValidateStrongPassword(string password)
        {
            string regexPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$";
            bool passed = Regex.IsMatch(password, regexPattern);
            if (!passed)
            {
                throw new Exception("Password must be at least 8 characters long and contain capital letters, lowercase letters, and numbers.");
            }
        }

        public static void ValidateNonEmptyStrings(string v)
        {
            throw new NotImplementedException();
        }
    }
}
