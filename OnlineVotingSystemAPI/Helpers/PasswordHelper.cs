using System.Security.Cryptography;
using System.Text;

namespace OnlineVotingSystemAPI.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            //generate salt
            using (var hmac = new HMACSHA256())
            {
                //use HMACSHA key as salt 
                var salt = hmac.Key;

                var passwordBytes = Encoding.UTF8.GetBytes(password);

                var hash = hmac.ComputeHash(passwordBytes);

                //combine Salt and Hash, and store
                return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split(':');

            var salt = Convert.FromBase64String(parts[0]);
            
            var storedPasswordHash = Convert.FromBase64String(parts[1]);

            using (var hmac = new HMACSHA256(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hash = hmac.ComputeHash(passwordBytes);

                return hash.SequenceEqual(storedPasswordHash);
            }
        }
    }
}
