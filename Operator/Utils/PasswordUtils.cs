namespace OP.Utils
{
    public class PasswordUtils
    {
        public static bool IsMatch(string clientPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(clientPassword, passwordHash);
        }

        public static string GenerateHash(string password)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(); // Generate a random salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt); // Generate the hash

            return hashedPassword;
        }


    }
}
