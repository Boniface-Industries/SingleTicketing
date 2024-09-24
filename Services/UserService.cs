using Microsoft.AspNetCore.Identity;
using SingleTicketing.Data;

namespace SingleTicketing.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context; // Replace MyDbContext with your actual DbContext
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(MyDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task CreateUserAsync(User user, string password)
        {
            // Hash the password
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user != null)
            {
                // Verify the password
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return result == PasswordVerificationResult.Success;
            }
            return false;
        }
        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }

}
