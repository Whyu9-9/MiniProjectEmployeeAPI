using Ardalis.Specification;
using EmployeeApi.Models;

namespace employee.Specification.AdminSpecification
{
    public class SearchByUsernameSpec : Specification<Admin>, ISingleResultSpecification
    {
        public SearchByUsernameSpec(string username)
        {
            Query.Where(admin => admin.Username == username);
        }
    }
}
