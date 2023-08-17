using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.DTOs.Incoming;
using employee.Repository.EmployeeRepository;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepo;

        public EmployeeController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        // GET: api/Employee
        [Authorize]
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var results = _employeeRepo.GetEmployee();

            return Ok(results);
        }

        // GET: api/Employee/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetEmployee(uint id)
        {
            var result = _employeeRepo.GetEmployeeById(id);

            return Ok(result);
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public IActionResult PostEmployee(EmployeeForCreationDto data)
        {
            var employee = _employeeRepo.PostEmployee(data);

            return Ok(employee);
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutEmployee(uint id, EmployeeForCreationDto data)
        {
            var result = _employeeRepo.PutEmployee(id, data);

            if(result == null)
            {
                return BadRequest("ID not found!");
            }

            return Ok(result);
        }

        // DELETE: api/Employee/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(uint id)
        {
            var result = _employeeRepo.DeleteEmployee(id);

            if (result == null)
            {
                return BadRequest("ID not found!");
            }

            var response = new
            {
                Message = "Employee Deleted!",
                DeletedEmployee = result
            };

            return Ok(response);
        }        
    }
}
