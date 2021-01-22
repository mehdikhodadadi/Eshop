using Middleware;
using System;

namespace IdentityModel
{
    public class User
    {
        public static readonly string DocumentName = "users";

        public string Email { get; set; }
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public void SetPassword(string password, IEncryptor encryptor)
        {
            Salt = encryptor.GetSalt(password);
            Password = encryptor.GetHash(password, Salt);
        }

        public bool ValidatePassword(string password, IEncryptor encryptor)
        {
            var isValid = Password.Equals(encryptor.GetHash(password, Salt));
            return isValid;
        }
    }
}