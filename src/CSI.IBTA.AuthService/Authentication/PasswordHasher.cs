using CSI.IBTA.AuthService.Interfaces;
using System.Security.Cryptography;

namespace CSI.IBTA.AuthService.Authentication
{
    internal class PasswordHasher : IPasswordHasher
    {
        private readonly int _saltByteSize = 24;
        private readonly int _hashByteSize = 24;
        private readonly int _pbkdf2Iterations = 1000;

        private readonly int _iterationIndex = 0;
        private readonly int _saltIndex = 1;
        private readonly int _pbkdf2Index = 2;

        public string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_saltByteSize);
            byte[] hash = PBKDF2(password, salt, _pbkdf2Iterations, _hashByteSize);

            return _pbkdf2Iterations + ":" +
                Convert.ToBase64String(salt) + ":" +
                Convert.ToBase64String(hash);
        }

        public bool Verify(string password, string correctHash)
        {
            char[] delimiter = [':'];
            string[] split = correctHash.Split(delimiter);
            int iterations = int.Parse(split[_iterationIndex]);
            byte[] salt = Convert.FromBase64String(split[_saltIndex]);
            byte[] hash = Convert.FromBase64String(split[_pbkdf2Index]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
