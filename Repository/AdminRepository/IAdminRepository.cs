using EmployeeApi.DTOs.Incoming;
using EmployeeApi.DTOs.Outgoing;

namespace employee.Repository.AdminRepository
{
    public interface IAdminRepository
    {
        IEnumerable<AdminDto> GetAdmin();
        AdminDto GetAdminById(uint id);
        AdminForCreationDto GetAdminByUsername(string username);
        AdminDto? PutAdmin(uint id, AdminForEditDto data);
        AdminDto? PostAdmin(AdminForCreationDto data);
        AdminDto? DeleteAdmin(uint id);
    }
}

//using EmployeeApi.DTOs.Incoming;
//using EmployeeApi.DTOs.Outgoing;

//namespace employee.Repository.AdminRepository
//{
//    public interface IAdminRepository
//    {
//        IEnumerable<AdminDto> GetAdmin();
//        Task<AdminDto> GetAdminById(uint id);
//        Task<AdminForCreationDto>? GetAdminByUsername(string username);
//        Task<AdminDto>? PutAdmin(uint id, AdminForEditDto data);
//        Task<AdminDto>? PostAdmin(AdminForCreationDto data);
//        Task<AdminDto>? DeleteAdmin(uint id);
//    }
//}

