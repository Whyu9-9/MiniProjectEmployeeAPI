using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeApi.Models.DTOs.Incoming;
using EmployeeApi.Models.DTOs.Outgoing;
using AutoMapper;
using EmployeeApi.Models;
using employee.Specification.ProjectSpecification;
using employee.Specification.EmployeeSpecification;
using employee.Models.DTOs.Incoming;

namespace employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IRepository<Project> _projectRepo;
        private readonly IRepository<Employee> _employeeRepo;
        private readonly IMapper _mapper;

        public ProjectController(IRepository<Project> projectRepo, IRepository<Employee> employeeRepo, IMapper mapper)
        {
            _projectRepo = projectRepo;
            _mapper = mapper;
            _employeeRepo = employeeRepo;
        }

        // GET: api/Project
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProjectsAsync()
        {
            var fetchs = await _projectRepo.ListAsync();
            var results = _mapper.Map<IEnumerable<ProjectDto>>(fetchs);

            return Ok(results);
        }

        // GET: api/Project/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectAsync(uint id)
        {
            var fetch = await _projectRepo.GetBySpecAsync(new ProjectWithEmployeListSpec(id));
            var result = _mapper.Map<ProjectDto>(fetch);

            return Ok(result);
        }

        // POST: api/Project
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostProjectAsync(ProjectForCreationDto data)
        {
            var mappedInput = _mapper.Map<Project>(data);
            var project = await _projectRepo.AddAsync(mappedInput);
            var result = _mapper.Map<ProjectDtoAfterCreate>(project);

            return Ok(result);
        }

        // POST: api/Project/project-employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("project-employee")]
        public async Task<IActionResult> PostProjectEmployeeAsync(ProjectEmployeeForCreationDto data)
        {
            var fetch = await _projectRepo.GetByIdAsync(data.ProjectId);

            if (fetch == null)
            {
                return NotFound();
            }

            var fetchEmployees = await _employeeRepo.ListAsync(new EmployeeByIdsSpec(data.EmployeeId));

            fetch.Employees = fetchEmployees;

            await _projectRepo.UpdateAsync(fetch);

            var result = _mapper.Map<ProjectDto>(fetch);

            return Ok(result);
        }

        // PUT: api/Project/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectAsync(uint id, ProjectForCreationDto data)
        {
            var existingProject = await _projectRepo.GetByIdAsync(id);

            if (existingProject == null)
            {
                return BadRequest("ID not found!");
            }

            var mappedInput = _mapper.Map(data, existingProject);

            await _projectRepo.UpdateAsync(mappedInput);

            return Ok(_mapper.Map<ProjectDtoAfterCreate>(mappedInput));
        }

        // PUT: api/Project/5/project-employee
        [Authorize]
        [HttpPut("{id}/project-employee")]
        public async Task<IActionResult> PutProjectEmployeeAsync(uint id, ProjectEmployeeForUpdateDto data)
        {
            var fetch = await _projectRepo.GetByIdAsync(id);

            if (fetch == null)
            {
                return BadRequest();
            }

            var fetchEmployees = await _employeeRepo.ListAsync(new EmployeeByIdsSpec(data.EmployeeId));

            fetch.Employees = fetchEmployees;

            await _projectRepo.UpdateAsync(fetch);

            var result = _mapper.Map<ProjectDto>(fetch);

            return Ok(result);
        }

        // DELETE: api/Project/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectAsync(uint id)
        {
            var result = await _projectRepo.GetByIdAsync(id);

            if (result == null)
            {
                return BadRequest("ID not found!");
            }

            await _projectRepo.DeleteAsync(result);

            return Ok("Project Deleted!");
        }
    }
}
