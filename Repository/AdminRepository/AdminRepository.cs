using AutoMapper;
using employee.Repository.AdminRepository;
using EmployeeApi.Data;
using EmployeeApi.DTOs.Incoming;
using EmployeeApi.DTOs.Outgoing;
using EmployeeApi.Models;

namespace employee.Repository.EmployeeRepository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly EmployeeApiContext _context;
        private readonly IMapper _mapper;

        public AdminRepository(EmployeeApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<AdminDto> GetAdmin()
        {
            var _admin = _context.Admins.ToList();
            var result = _mapper.Map<IEnumerable<AdminDto>>(_admin);

            return result;
        }

        public AdminDto GetAdminById(uint id)
        {
            var _admin = _context.Admins.Find(id);
            var result = _mapper.Map<AdminDto>(_admin);

            return result;
        }

        public AdminForCreationDto GetAdminByUsername(string username)
        {
            var _admin = _context.Admins.FirstOrDefault(a => a.Username == username);
            var result = _mapper.Map<AdminForCreationDto>(_admin);

            return result;
        }

        public AdminDto? PostAdmin(AdminForCreationDto data)
        {
            var existingAdmin = GetAdminByUsername(data.Username);

            if (existingAdmin != null)
            {
                return null;
            }

            var _admin = _mapper.Map<Admin>(data);

            _context.Admins.Add(_admin);

            _context.SaveChanges();

            var result = _mapper.Map<AdminDto>(_admin);

            return result;
        }

        public AdminDto? PutAdmin(uint id, AdminForEditDto data)
        {
            var existingAdmin = _context.Admins.Find(id);

            if (existingAdmin == null)
            {
                return null;
            }

            var result = _mapper.Map(data, existingAdmin);

            _context.SaveChanges();

            return _mapper.Map<AdminDto>(result);
        }

        public AdminDto? DeleteAdmin(uint id)
        {
            var existingAdmin = _context.Admins.Find(id);

            if (existingAdmin == null)
            {
                return null;
            }

            _context.Admins.Remove(existingAdmin);
            _context.SaveChanges();

            return _mapper.Map<AdminDto>(existingAdmin);
        }
    }
}

//using AutoMapper;
//using employee.Repository.AdminRepository;
//using EmployeeApi.Data;
//using EmployeeApi.DTOs.Incoming;
//using EmployeeApi.DTOs.Outgoing;
//using EmployeeApi.Models;

//namespace employee.Repository.EmployeeRepository
//{
//    public class AdminRepository : IAdminRepository
//    {
//        private readonly IRepository<Admin> _adminRepository;
//        private readonly IMapper _mapper;

//        public AdminRepository(IRepository<Admin> adminRepository, IMapper mapper)
//        {
//            _adminRepository = adminRepository;
//            _mapper = mapper;
//        }

//        public IEnumerable<AdminDto> GetAdmin()
//        {
//            var admins = _adminRepository.ListAsync();
//            var result = _mapper.Map<IEnumerable<AdminDto>>(admins);

//            return result;
//        }

//        public async Task<AdminDto> GetAdminById(uint id)
//        {
//            var admin = await _adminRepository.GetByIdAsync(id);
//            var result = _mapper.Map<AdminDto>(admin);

//            return result;
//        }

//        public async Task<AdminForCreationDto> GetAdminByUsername(string username)
//        {
//            var spec = new AdminsWithUsernameSpecification(username);
//            var admin = await _adminRepository.GetBySpecAsync(spec);
//            var result = _mapper.Map<AdminForCreationDto>(admin);

//            return result;
//        }

//        public async Task<AdminDto>? PostAdmin(AdminForCreationDto data)
//        {
//            var existingAdmin = await GetAdminByUsername(data.Username);

//            if (existingAdmin != null)
//            {
//                return null;
//            }

//            var admin = _mapper.Map<Admin>(data);

//            admin = await _adminRepository.AddAsync(admin);

//            var result = _mapper.Map<AdminDto>(admin);

//            return result;
//        }

//        public async Task<AdminDto> PutAdmin(uint id, AdminForEditDto data)
//        {
//            var existingAdminSpec = new AdminsWithIdSpecification(id);
//            var existingAdmin = await _adminRepository.GetBySpecAsync(existingAdminSpec);

//            if (existingAdmin == null)
//            {
//                return null;
//            }

//            _mapper.Map(data, existingAdmin);
//            await _adminRepository.UpdateAsync(existingAdmin);

//            return _mapper.Map<AdminDto>(existingAdmin);
//        }

//        public async Task<AdminDto>? DeleteAdmin(uint id)
//        {
//            var existingAdminSpec = new AdminsWithIdSpecification(id);
//            var existingAdmin = await _adminRepository.GetBySpecAsync(existingAdminSpec);

//            if (existingAdmin == null)
//            {
//                return null;
//            }

//            await _adminRepository.DeleteAsync(existingAdmin);

//            return _mapper.Map<AdminDto>(existingAdmin);
//        }
//    }
//}