using Ardalis.Specification;
using EmployeeApi.Models;

namespace employee.Specification.ProjectSpecification
{
    public class ProjectWithEmployeListSpec : Specification<Project>, ISingleResultSpecification
    {
        public ProjectWithEmployeListSpec(uint projectId)
        {
            Query
                .Where(p => p.Id == projectId)
                .Include(p => p.Employees);
        }
    }
}

