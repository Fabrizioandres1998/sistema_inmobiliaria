namespace InmobiliariaTPI.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string HashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, HashedPassword);
            }
            catch
            {
                return false;
            }
        }
    }
}