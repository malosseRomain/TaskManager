using System.Security.Cryptography;
using System.Text;
using TaskMaster.Models;
using TaskMaster.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(string nom, string prenom, string email, string password);
        Task<bool> LoginAsync(string email, string password);
        void Logout();
        bool IsAuthenticated { get; }
        User? CurrentUser { get; }
        event EventHandler AuthStateChanged;
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private User? _currentUser;
        public event EventHandler? AuthStateChanged;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public bool IsAuthenticated => _currentUser != null;
        public User? CurrentUser => _currentUser;

        public async Task<bool> RegisterAsync(string nom, string prenom, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return false;

            var user = new User
            {
                Nom = nom,
                Prenom = prenom,
                Email = email,
                Password = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return false;

            _currentUser = user;
            AuthStateChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }


        public void Logout()
        {
            _currentUser = null;
            AuthStateChanged?.Invoke(this, EventArgs.Empty);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
} 