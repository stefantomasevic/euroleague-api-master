using Euroleague.Data;
using Euroleague.Models;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Euroleague.Repository
{
    public class SqlAuthorizeRepository : IAuthorize
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public SqlAuthorizeRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<string> GenerateToken(User user)
        {
            var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: credentials
            );

            return  new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> LogIn(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            return user;
        }

        public async Task<User> SignIn(string username, string password, string email)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (existingUser != null)
                throw new Exception("Username already exists");  // If the user already exists, throw an error

            // Hash the password before saving it
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new user object
            var newUser = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Email = email,
                FullName = username
            };

            // Add the new user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();  // Save changes to the database

            return newUser;
        }
    }
}
