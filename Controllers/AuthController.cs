using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.DTOs.Incoming;
using EmployeeApi.Models;
using employee.Specification.AdminSpecification;
using AutoMapper;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private readonly IRepository<Admin> _adminRepo;
        private IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IRepository<Admin> adminRepo, IMapper mapper , IConfiguration configuration)
        {
            _adminRepo = adminRepo;
            _config = configuration;
            _mapper = mapper;
        }

        // POST: api/Auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AdminForLoginDto data)
        {
            var admin = await _adminRepo.GetBySpecAsync(new SearchByUsernameSpec(data.Username));

            if (admin != null)
            {
                if (VerifyPassword(data.Password, admin.Password, HexStringToByteArray(admin.Salt)))
                {
                    return Ok(new { Token = GenerateToken(_mapper.Map<AdminForCreationDto>(admin)) });
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
                            expires: DateTime.Now.AddMinutes(9999),
                            signingCredentials: credentials
                        );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
