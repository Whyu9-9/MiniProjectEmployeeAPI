using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using employee.Repository.EmployeeRepository;
using EmployeeApi.Models.DTOs.Incoming;
using EmployeeApi.Models.DTOs.Outgoing;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepo;

        public ProjectController(IProjectRepository projectRepo)
        {
            _projectRepo = projectRepo;
        }

        // GET: api/Project
        [Authorize]
        [HttpGet]
        public IActionResult GetProjects()
        {
            var results = _projectRepo.GetProject();

            return Ok(results);
        }

        // GET: api/Project/5
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetProject(uint id)
        {
            var result = _projectRepo.GetProjectById(id);

            return Ok(result);
        }

        // POST: api/Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public IActionResult PostProject(ProjectForCreationDto data)
        {
            var employee = _projectRepo.PostProject(data);

            return Ok(employee);
        }

        // POST: api/Project/project-employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("project-employee")]
        public IActionResult PostProjectEmployee(ProjectEmployeeForCreationDto data)
        {
            var projectDto = _projectRepo.PostProjectEmployee(data);

            if (projectDto == null)
            {
                // Handle the case where the project doesn't exist
                return NotFound();
            }

            return Ok(projectDto);
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult PutProject(uint id, ProjectForCreationDto data)
        {
            var result = _projectRepo.PutProject(id, data);

            if(result == null)
            {
                return BadRequest("ID not found!");
            }

            return Ok(result);
        }

        // PUT: api/Project/5/project-employee
        [Authorize]
        [HttpPut("{id}/project-employee")]
        public IActionResult PutProjectEmployee(uint id, ProjectEmployeeForCreationDto data)
        {
            var projectDto = _projectRepo.PutProjectEmployee(id, data);

            if (projectDto == null)
            {
                return NotFound();
            }

            return Ok(projectDto);
        }

        // DELETE: api/Project/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(uint id)
        {
            var result = _projectRepo.DeleteProject(id);

            if (result == null)
            {
                return BadRequest("ID not found!");
            }

            return Ok("Project Deleted!");
        }        
    }
}
