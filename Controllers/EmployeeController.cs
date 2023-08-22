using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.DTOs.Incoming;
using EmployeeApi.Models;
using AutoMapper;
using EmployeeApi.DTOs.Outgoing;
using System;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IMapper _mapper;

        public EmployeeController(IRepository<Employee> employeeRepo, IMapper mapper)
        {
            _employeeRepo = employeeRepo;
            _mapper = mapper;
        }

        // GET: api/Employee
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetEmployeesAsync([FromQuery] string? range = null)
        {
            var fetchs = await _employeeRepo.ListAsync();

            int startIndex = 0;
            int endIndex = fetchs.Count() - 1;

            if (!string.IsNullOrEmpty(range))
            {
                var rangeParts = range.Split('-');
                if (rangeParts.Length == 2)
                {
                    startIndex = int.Parse(rangeParts[0]);
                    endIndex = int.Parse(rangeParts[1]);
                }
            }

            var pagedResults = fetchs.Skip(startIndex).Take(endIndex - startIndex + 1);
            var results = _mapper.Map<IEnumerable<EmployeeDto>>(pagedResults);

            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Range");
            Response.Headers.Add("Content-Range", $"{startIndex}-{endIndex}/{fetchs.Count()}");

            return Ok(results);
        }

        // GET: api/Employee/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeAsync(uint id)
        {
            var fetch = await _employeeRepo.GetByIdAsync(id);
            var result = _mapper.Map<EmployeeDto>(fetch);

            return Ok(result);
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostEmployeeAsync(EmployeeForCreationDto data)
        {
            var mappedInput = _mapper.Map<Employee>(data);
            var employee = await _employeeRepo.AddAsync(mappedInput);
            var result = _mapper.Map<EmployeeDto>(employee);

            return Ok(result);
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeAsync(uint id, EmployeeForCreationDto data)
        {
            var existingEmployee = await _employeeRepo.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                return BadRequest("ID not found!");
            }

            var mappedInput = _mapper.Map(data, existingEmployee);

            await _employeeRepo.UpdateAsync(mappedInput);

            return Ok(_mapper.Map<EmployeeDto>(mappedInput));
        }

        // DELETE: api/Employee/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(uint id)
        {
            var existingEmployee = await _employeeRepo.GetByIdAsync(id);

            if (existingEmployee == null)
            {
                return BadRequest("ID not found!");
            }

            await _employeeRepo.DeleteAsync(existingEmployee);

            var response = new
            {
                Message = "Employee Deleted!",
                DeletedEmployee = _mapper.Map<EmployeeDto>(existingEmployee)
            };

            return Ok(response);
        }        
    }
}
