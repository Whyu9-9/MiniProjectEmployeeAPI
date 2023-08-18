using Ardalis.Specification;
using EmployeeApi.Models;

namespace employee.Specification.EmployeeSpecification
{
	public class EmployeeByIdsSpec : Specification<Employee>, ISingleResultSpecification
    {
		public EmployeeByIdsSpec(List<uint> ids)
		{
			Query.Where(e => ids.Contains(e.Id));
		}
	}
}

