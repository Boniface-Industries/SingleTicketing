using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using SingleTicketing.Data;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SingleTicketing.Services
{
    public class UserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public User CreateUser(string username, string password, string role)
        {
            var user = new User
            {
                Username = username,
                PasswordHash = _passwordHasher.HashPassword(null, password), // Hashing password
                Role = role
            };

            return user;
        }
    }
}
