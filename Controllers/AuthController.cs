using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeApi.Data;
using EmployeeApi.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private readonly EmployeeApiContext _context;
        private IConfiguration _config;

        public AuthController(EmployeeApiContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        // POST: api/Auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(Admin admin)
        {
            if (_context.Admins == null)
            {
               return Problem("Entity set 'EmployeeApiContext.Admins' is null.");
            }

            var hashedPass = HashPasword(admin.Password, out var salt);
            admin.Password = hashedPass;
            admin.Salt = Convert.ToHexString(salt);

            var check = await _context.Admins.FirstOrDefaultAsync(a => a.Username == admin.Username);

            if (check != null)
            {
                return Problem("Admin already exist!");
            }

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return "Bearer " + GenerateToken(admin);
        }

        // POST: api/Auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string username, string password)
        {
            if (_context.Admins == null)
            {
                return NotFound();
            }
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);

            if (admin == null)
            {
                return NotFound();
            }

            if (!VerifyPassword(password, admin.Password, HexStringToByteArray(admin.Salt)))
            {
                return BadRequest();
            }

            return "Bearer " + GenerateToken(admin);
        }

        private string HashPasword(string password, out byte[] salt)
        {
           salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
            hashAlgorithm,
                keySize);

            return Convert.ToHexString(hash);
        }

        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }

        private byte[] HexStringToByteArray(string hexString)
        {
            byte[] byteArray = new byte[hexString.Length / 2];

            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return byteArray;
        }

        private string GenerateToken(Admin admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                            _config["Jwt:Issuer"],
                            _config["Jwt:Audience"],
                            null,
                            expires: DateTime.Now.AddMinutes(1),
                            signingCredentials: credentials
                        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
