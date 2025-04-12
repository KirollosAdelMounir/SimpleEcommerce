using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceCore
{
    public class Encryptions
    {
        // Generates a hash for the password with a random salt
        public static string HashPassword(string password)
        {
            // Generate a salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Combine password and salt
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, passwordWithSaltBytes, 0, salt.Length);
                Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, salt.Length, passwordBytes.Length);

                // Compute the hash
                byte[] hashBytes = sha256.ComputeHash(passwordWithSaltBytes);

                // Combine salt and hash for storage
                byte[] hashWithSaltBytes = new byte[salt.Length + hashBytes.Length];
                Buffer.BlockCopy(salt, 0, hashWithSaltBytes, 0, salt.Length);
                Buffer.BlockCopy(hashBytes, 0, hashWithSaltBytes, salt.Length, hashBytes.Length);

                // Convert to Base64 for storage
                return Convert.ToBase64String(hashWithSaltBytes);
            }
        }

        // Validates the password against the stored hash
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashWithSaltBytes = Convert.FromBase64String(storedHash);

            // Extract salt
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashWithSaltBytes, 0, salt, 0, salt.Length);

            // Hash the entered password with the extracted salt
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(enteredPassword);
                byte[] passwordWithSaltBytes = new byte[passwordBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, passwordWithSaltBytes, 0, salt.Length);
                Buffer.BlockCopy(passwordBytes, 0, passwordWithSaltBytes, salt.Length, passwordBytes.Length);

                // Compute hash
                byte[] hashBytes = sha256.ComputeHash(passwordWithSaltBytes);

                // Extract stored hash
                byte[] storedHashBytes = new byte[hashWithSaltBytes.Length - salt.Length];
                Buffer.BlockCopy(hashWithSaltBytes, salt.Length, storedHashBytes, 0, storedHashBytes.Length);

                // Compare
                return CryptographicOperations.FixedTimeEquals(hashBytes, storedHashBytes);
            }
        }
    }
}
