using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using webkino.Data;
using webkino.Models.Entities;
using webkino.Services.Interfaces;
using webkino.ViewModels;

namespace webkino.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Register(UserRegisterViewModel model, out string errorMessage)
        {
            errorMessage = string.Empty;

            var emailExists = _context.Users.Any(u => u.Email == model.Email);
            if (emailExists)
            {
                errorMessage = "Користувач з таким email вже існує.";
                return false;
            }

            var usernameExists = _context.Users.Any(u => u.Username == model.Username);
            if (usernameExists)
            {
                errorMessage = "Користувач з таким іменем вже існує.";
                return false;
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            SetUserSession(user);

            return true;
        }

        public bool Login(UserLoginViewModel model, out string errorMessage)
        {
            errorMessage = string.Empty;

            var passwordHash = HashPassword(model.Password);

            var user = _context.Users
                .FirstOrDefault(u => u.Email == model.Email && u.PasswordHash == passwordHash);

            if (user == null)
            {
                errorMessage = "Невірний email або пароль.";
                return false;
            }

            SetUserSession(user);

            return true;
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }

        public UserProfileViewModel? GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");

            if (userId == null)
            {
                return null;
            }

            return _context.Users
                .Where(u => u.Id == userId.Value)
                .Select(u => new UserProfileViewModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Role = u.Role
                })
                .FirstOrDefault();
        }

        private void SetUserSession(User user)
        {
            _httpContextAccessor.HttpContext?.Session.SetInt32("UserId", user.Id);
            _httpContextAccessor.HttpContext?.Session.SetString("Username", user.Username);
            _httpContextAccessor.HttpContext?.Session.SetString("UserRole", user.Role);
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(
                SHA256.HashData(Encoding.UTF8.GetBytes(password))
            );
        }
    }
}