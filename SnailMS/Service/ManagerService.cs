using CRUD;
using DataBase.Entity;
using SnailMS.Models;

namespace SnailMS.Service
{
    public class ManagerService
    {
        private Data data;
        public ManagerService(Data _data)
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
        public ManagerDto ConvertManagerToDto(Manager manager)
        {
            return new ManagerDto
            {
                Id = manager.Id,
                FirstName = manager.FirstName,
                SecondName = manager.SecondName,
                LastName = manager.LastName,
                Login = manager.Login,
                Password = Base64Decode(manager.Password),
                DepartmentName = manager.DepartmentName
            };
        }
        public Manager ConvertDtoToManager(ManagerDto managerDto)
        {
            return new Manager
            {
                Id = managerDto.Id,
                FirstName = managerDto.FirstName,
                SecondName = managerDto.SecondName,
                LastName = managerDto.LastName,
                Login = managerDto.Login,
                Password = Base64Encode(managerDto.Password),
                DepartmentName = managerDto.DepartmentName
            };
        }

        public IEnumerable<ManagerDto> GetAllManagerDto()
        {
            List<ManagerDto> managerDtos = new List<ManagerDto>();
            List<Manager> managers = data.Managers.GetAllManagers().ToList();
            if (managers != null)
            {
                foreach (Manager manager in managers)
                {
                    managerDtos.Add(ConvertManagerToDto(manager));
                }
            }
            return managerDtos;
        }
        public ManagerDto GetManagerDtoById(string id)
        {
            return ConvertManagerToDto(data.Managers.GetManagerById(id));
        }
        public void DeleteManagerById(string id)
        {
            data.Managers.DeleteManagerById(id);
        }
        public void SaveManagerDto(ManagerDto managertoToSave)
        {
            data.Managers.SaveManager(ConvertDtoToManager(managertoToSave));
        }
    }
}