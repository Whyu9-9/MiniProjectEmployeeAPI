using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using employee.Repository.AdminRepository;
using EmployeeApi.DTOs.Incoming;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private readonly IAdminRepository _adminRepo;
        private IConfiguration _config;

        public AuthController(IAdminRepository adminRepo, IConfiguration configuration)
        {
            _adminRepo = adminRepo;
            _config = configuration;
        }

        // POST: api/Auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<string> Login(string username, string password)
        {
            var admin = _adminRepo.GetAdminByUsername(username);

            if (admin != null)
            {
                if (VerifyPassword(password, admin.Password, HexStringToByteArray(admin.Salt)))
                {
                    return "Bearer " + GenerateToken(admin);
                }

                return BadRequest();
            }

            return NotFound();
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

        private string GenerateToken(AdminForCreationDto admin)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: _config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                            admin.Username,
                            _config["Jwt:Audience"],
                            null,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: credentials
                        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
