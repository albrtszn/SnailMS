using CRUD;
using DataBase.Entity;
using SnailMS.Models;

namespace SnailMS.Service
{
    public class AdminService
    {
        private Data data;
        public AdminService(Data _data)
        {
            data = _data;
        }
        // Base64 logic
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        //
        public AdminDto ConvertAdminToDto(Admin admin)
        {
            return new AdminDto
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                SecondName = admin.SecondName,
                LastName = admin.LastName,
                Login = admin.Login,
                Password = Base64Decode(admin.Password),
                DepartmentName = admin.DepartmentName
            };
        }
        public Admin ConvertDtoToAdmin(AdminDto adminDto)
        {
            return new Admin
            {
                Id = adminDto.Id,
                FirstName = adminDto.FirstName,
                SecondName = adminDto.SecondName,
                LastName = adminDto.LastName,
                Login = adminDto.Login,
                Password = Base64Encode(adminDto.Password),
                DepartmentName = adminDto.DepartmentName
            };
        }

        public IEnumerable<AdminDto> GetAllAdminDto()
        {
            List<AdminDto> adminDtos = new List<AdminDto>();
            List<Admin> admins = data.Admins.GetAllAdmins().ToList();
            if (admins != null)
            {
                foreach (Admin admin in admins)
                {
                    adminDtos.Add(ConvertAdminToDto(admin));
                }
            }
            return adminDtos;
        }
        public AdminDto GetAdminDtoById(string id)
        {
            return ConvertAdminToDto(data.Admins.GetAdminById(id));
        }
        public void DeleteADMINById(string id)
        {
            data.Admins.DeleteAdminById(id);
        }
        public void SaveAdminDto(AdminDto adminDtoToSave)
        {
            data.Admins.SaveAdmin(ConvertDtoToAdmin(adminDtoToSave));
        }
    }
}
