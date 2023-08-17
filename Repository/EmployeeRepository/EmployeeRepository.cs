using AutoMapper;
using EmployeeApi.Data;
using EmployeeApi.DTOs.Incoming;
using EmployeeApi.DTOs.Outgoing;
using EmployeeApi.Models;

namespace employee.Repository.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeApiContext _context;
        private readonly IMapper _mapper;

        public EmployeeRepository(EmployeeApiContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<EmployeeDto> GetEmployee()
        {
            var _employees = _context.Employees.ToList();
            var result = _mapper.Map<IEnumerable<EmployeeDto>>(_employees);

            return result; 
        }

        public EmployeeDto GetEmployeeById(uint id)
        {
            var _employee = _context.Employees.Find(id);
            var result = _mapper.Map<EmployeeDto>(_employee);

            return result;
        }

        public EmployeeDto PostEmployee(EmployeeForCreationDto data)
        {
            var _employee = _mapper.Map<Employee>(data);
           
            _context.Employees.Add(_employee);

            _context.SaveChanges();

            var result = _mapper.Map<EmployeeDto>(_employee);

            return result;
        }

        public EmployeeDto? PutEmployee(uint id, EmployeeForCreationDto data)
        {
            var existingEmployee = _context.Employees.Find(id);

            if (existingEmployee == null)
            {
                return null;
            }

            var result = _mapper.Map(data, existingEmployee);

            _context.SaveChanges();

            return _mapper.Map<EmployeeDto>(result);
        }

        public EmployeeDto? DeleteEmployee(uint id)
        {
            var existingEmployee = _context.Employees.Find(id);

            if (existingEmployee == null)
            {
                return null;
            }

            _context.Employees.Remove(existingEmployee);
            _context.SaveChanges();

            return _mapper.Map<EmployeeDto>(existingEmployee);
        }
    }
}

