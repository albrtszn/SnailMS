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

        public ManagerDto ConvertManagerToDto(Manager manager)
        {
            return new ManagerDto
            {
                Id = manager.Id,
                FirstName = manager.FirstName,
                SecondName = manager.SecondName,
                LastName = manager.LastName,
                Login = manager.Login,
                Password = manager.Password,
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
                Password = managerDto.Password,
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