using AutoMapper;
using EmployeeApi.Data;
using EmployeeApi.Models;
using EmployeeApi.Models.DTOs.Incoming;
using EmployeeApi.Models.DTOs.Outgoing;
using Microsoft.EntityFrameworkCore;

namespace employee.Repository.EmployeeRepository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly EmployeeApiContext _context;
        private readonly IMapper _mapper;

        public ProjectRepository(EmployeeApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<ProjectDto> GetProject()
        {
            var _projects = _context.Projects.ToList();
            var result = _mapper.Map<IEnumerable<ProjectDto>>(_projects);

            return result;
        }

        public ProjectDto? GetProjectById(uint id)
        {
            var _project = _context.Projects
                .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee) 
                .FirstOrDefault(p => p.Id == id);

            if (_project == null)
            {
                return null;
            }

            var projectDto = _mapper.Map<ProjectDto>(_project);

            return projectDto;
        }

        public ProjectDto PostProject(ProjectForCreationDto data)
        {
            var _project = _mapper.Map<Project>(data);
           
            _context.Projects.Add(_project);

            _context.SaveChanges();

            var result = _mapper.Map<ProjectDto>(_project);

            return result;
        }

        public ProjectDto? PostProjectEmployee(ProjectEmployeeForCreationDto data)
        {
            var existingProject = _context.Projects
                .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee)
                .FirstOrDefault(p => p.Id == data.ProjectId);

            if (existingProject == null)
            {
                return null;
            }

            var newProjectEmployee = _mapper.Map<ProjectEmployee>(data);

            existingProject.ProjectEmployees.Add(newProjectEmployee);

            _context.SaveChanges();

            var result = _mapper.Map<ProjectDto>(existingProject);

            return result;
        }

        public ProjectDto? PutProject(uint id, ProjectForCreationDto data)
        {
            var existingProject = _context.Projects
               .Include(p => p.ProjectEmployees)
               .ThenInclude(pe => pe.Employee)
               .FirstOrDefault(p => p.Id == id);

            if (existingProject == null)
            {
                return null;
            }

            var result = _mapper.Map(data, existingProject);

            _context.SaveChanges();

            return _mapper.Map<ProjectDto>(result);
        }

        public string? PutProjectEmployee(uint id, ProjectEmployeeForCreationDto data)
        {
            var projectEmployee = _context.ProjectEmployees
                .Include(pe => pe.Employee)
                .FirstOrDefault(pe => pe.Id == id);

            if (projectEmployee == null)
            {
                return null;
            }

            _mapper.Map(data, projectEmployee);

            _context.SaveChanges();

            return "Success";
        }

        public ProjectDto? DeleteProject(uint id)
        {
            var existingProject = _context.Projects.Find(id);

            if (existingProject == null)
            {
                return null;
            }

            _context.Projects.Remove(existingProject);
            _context.SaveChanges();

            return _mapper.Map<ProjectDto>(existingProject);
        }
    }
}

