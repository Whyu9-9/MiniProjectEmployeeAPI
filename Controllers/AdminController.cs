using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.DTOs.Incoming;
using employee.Repository.AdminRepository;
using System.Security.Cryptography;
using System.Text;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private readonly IAdminRepository _adminRepo;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        // GET: api/Admin
        [Authorize]
        [HttpGet]
        public IActionResult GetAdmins()
        {
            var results = _adminRepo.GetAdmin();

            return Ok(results);
        }

        // GET: api/Admin/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetAdmin(uint id)
        {
            var result = _adminRepo.GetAdminById(id);

            return Ok(result);
        }

        // POST: api/Admin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public IActionResult PostAdmin(AdminForCreationDto data)
        {
            var hashedPass = HashPasword(data.Password, out var salt);

            data.Password = hashedPass;
            data.Salt = Convert.ToHexString(salt);

            var admin = _adminRepo.PostAdmin(data);

            if (admin == null)
            {
                return Problem("Admin already exist!");
            }

            return Ok(admin);
        }

        // PUT: api/Admin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutAdmin(uint id, AdminForEditDto data)
        {
            var result = _adminRepo.PutAdmin(id, data);

            if(result == null)
            {
                return BadRequest("ID not found!");
            }

            return Ok(result);
        }

        // DELETE: api/Admin/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin(uint id)
        {
            var result = _adminRepo.DeleteAdmin(id);

            if (result == null)
            {
                return BadRequest("ID not found!");
            }

            var response = new
            {
                Message = "Admin Deleted!",
                DeletedAdmin = result
            };

            return Ok(response);
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
    }
}
