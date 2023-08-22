using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.DTOs.Incoming;
using System.Security.Cryptography;
using System.Text;
using EmployeeApi.Models;
using EmployeeApi.DTOs.Outgoing;
using AutoMapper;
using employee.Specification.AdminSpecification;
using Microsoft.Extensions.Hosting;
using System.Composition;
using System.Reflection.PortableExecutable;
using System.Web;
using System.Globalization;

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
        //public async Task<IActionResult> GetAdmins([FromQuery] string filter, [FromQuery] string range, [FromQuery] string sort)
        //{
        //    var fetchs = await _adminRepo.ListAsync();

        //    // Apply filtering based on the 'filter' query parameter
        //    if (!string.IsNullOrEmpty(filter))
        //    {
        //        Console.WriteLine($"Filter Parameter: {filter}");
        //        fetchs = fetchs.Where(item => item.Username.Contains(filter)).ToList();
        //    }

        //    // Apply sorting based on the 'sort' query parameter
        //    if (!string.IsNullOrEmpty(sort))
        //    {
        //        var sortArray = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(HttpUtility.UrlDecode(sort));
        //        if (sortArray.Count == 2)
        //        {
        //            string sortField = sortArray[0];
        //            string sortOrder = sortArray[1];

        //            if (!string.IsNullOrEmpty(sortField) && (sortOrder == "ASC" || sortOrder == "DESC"))
        //            {
        //                if (sortField == "id" || sortField == "username") // Replace with actual property names
        //                {
        //                    string inputString = sortField;
        //                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //                    string titleCaseString = textInfo.ToTitleCase(inputString);

        //                    if (sortOrder == "ASC")
        //                    {
        //                        fetchs = fetchs.OrderBy(item => item.GetType().GetProperty(titleCaseString)?.GetValue(item)).ToList();
        //                    }
        //                    else
        //                    {
        //                        fetchs = fetchs.OrderByDescending(item => item.GetType().GetProperty(titleCaseString)?.GetValue(item)).ToList();
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    // Parse the 'range' query parameter to determine pagination
        //    int startIndex = 0;
        //    int endIndex = fetchs.Count() - 1;

        //    if (!string.IsNullOrEmpty(range))
        //    {
        //        var rangeParts = range.Trim('[', ']').Split(',');
        //        if (rangeParts.Length == 2 && int.TryParse(rangeParts[0], out int start) && int.TryParse(rangeParts[1], out int end))
        //        {
        //            startIndex = start;
        //            endIndex = Math.Min(end, fetchs.Count() - 1);
        //        }
        //    }

        //    var pagedResults = fetchs.Skip(startIndex).Take(endIndex - startIndex + 1);
        //    var results = _mapper.Map<IEnumerable<AdminDto>>(pagedResults);

        //    // Set the 'Content-Range' header for proper pagination information
        //    Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
        //    Response.Headers.Add("Content-Range", $"{startIndex}-{endIndex}/{fetchs.Count()}");

        //    // Return the paged and sorted results
        //    return Ok(results);
        //}
        public async Task<IActionResult> GetAdmins([FromQuery] string? filter = null, [FromQuery] string? sort = null, [FromQuery] int page = 1, [FromQuery] int perPage = 10)
        {
            var fetchs = await _adminRepo.ListAsync();

            // Apply filtering based on the 'filter' query parameter
            //if (!string.IsNullOrEmpty(filter) || HttpUtility.UrlDecode(filter) != "{}")
            //{
            //    fetchs = fetchs.Where(item => item.Username.Contains(filter)).ToList();
            //}

            //// Sorting based on the 'sort' query parameter
            //if (!string.IsNullOrEmpty(sort))
            //{
            //    if (sort == "ASC")
            //    {
            //        fetchs = fetchs.OrderBy(item => item.Id).ToList();
            //    }
            //    else if (sort == "DESC")
            //    {
            //        fetchs = fetchs.OrderByDescending(item => item.Id).ToList();
            //    }
            //}

            // Calculate the starting and ending indices for pagination
            int startIndex = (page - 1) * perPage;
            int endIndex = Math.Min(startIndex + perPage - 1, fetchs.Count() - 1);

            var pagedResults = fetchs.Skip(startIndex).Take(endIndex - startIndex + 1);
            var results = _mapper.Map<IEnumerable<AdminDto>>(pagedResults);

            // Set the Content-Range header for proper pagination information
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Add("Content-Range", $"{startIndex}-{endIndex}/{fetchs.Count()}");

            // Return the paged and sorted results
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
