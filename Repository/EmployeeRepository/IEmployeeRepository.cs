using EmployeeApi.DTOs.Incoming;
using EmployeeApi.DTOs.Outgoing;

namespace employee.Repository.EmployeeRepository
{
	public interface IEmployeeRepository
	{
        IEnumerable<EmployeeDto> GetEmployee();
        EmployeeDto GetEmployeeById(uint id);
        EmployeeDto? PutEmployee(uint id, EmployeeForCreationDto employee);
        EmployeeDto PostEmployee(EmployeeForCreationDto employee);
        EmployeeDto? DeleteEmployee(uint id);
    }
}

