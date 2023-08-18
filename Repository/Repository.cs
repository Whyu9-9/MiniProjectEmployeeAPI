using Ardalis.Specification.EntityFrameworkCore;
using EmployeeApi.Data;

namespace employee.Repository
{
	public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
	{
		public EfRepository(EmployeeApiContext context) : base(context){}
	}
}

