using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.DTOs.Incoming;
using System.Security.Cryptography;
using System.Text;
using EmployeeApi.Models;
using EmployeeApi.DTOs.Outgoing;
using AutoMapper;
using employee.Specification.AdminSpecification;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        private readonly IRepository<Admin> _adminRepo;
        private readonly IMapper _mapper;

        public AdminController(IRepository<Admin> adminRepo, IMapper mapper)
        {
            _adminRepo = adminRepo;
            _mapper = mapper;
        }

        // GET: api/Admin
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            var fetchs = await _adminRepo.ListAsync();
            var results = _mapper.Map<IEnumerable<AdminDto>>(fetchs);

            return Ok(results);
        }

        // GET: api/Admin/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdmin(uint id)
        {
            var fetch = await _adminRepo.GetByIdAsync(id);

            var result = _mapper.Map<AdminDto>(fetch);

            return Ok(result);
        }

        // POST: api/Admin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostAdminAsync(AdminForCreationDto data)
        {
            var existingAdmin = await _adminRepo.GetBySpecAsync(new SearchByUsernameSpec(data.Username));

            if (existingAdmin != null)
            {
                return Problem("Admin already exist!");
            }

            var hashedPass = HashPasword(data.Password, out var salt);

            data.Password = hashedPass;
            data.Salt = Convert.ToHexString(salt);

            var mappedInput = _mapper.Map<Admin>(data);
            var admin = await _adminRepo.AddAsync(mappedInput);
            var result = _mapper.Map<AdminDto>(admin);

            return Ok(result);
        }

        // PUT: api/Admin/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmin(uint id, AdminForEditDto data)
        {
            var existingAdmin = await _adminRepo.GetByIdAsync(id);

            if (existingAdmin == null)
            {
                return BadRequest("ID not found!");
            }

            var mappedInput = _mapper.Map(data, existingAdmin);

            await _adminRepo.UpdateAsync(mappedInput);

            return Ok(_mapper.Map<AdminDto>(mappedInput));
        }

        // DELETE: api/Admin/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminAsync(uint id)
        {
            var existingAdmin = await _adminRepo.GetByIdAsync(id);

            if (existingAdmin == null)
            {
                return BadRequest("ID not found!");
            }

            await _adminRepo.DeleteAsync(existingAdmin);

            var response = new
            {
                Message = "Admin Deleted!",
                DeletedAdmin = _mapper.Map<AdminDto>(existingAdmin)
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
                keySize
            );

            return Convert.ToHexString(hash);
        }
    }
}
