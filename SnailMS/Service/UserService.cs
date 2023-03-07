using CRUD;
using DataBase.Entity;
using SnailMS.Models;

namespace SnailMS.Service
{
    public class UserService
    {
        private Data data;
        public UserService(Data _data)
        {
            data = _data;
        }
        
        public UserDto ConvertUserToDto(User user)
        {
            return new UserDto 
            { 
                Id=user.Id, 
                FirstName=user.FirstName, 
                SecondName=user.SecondName,
                LastName=user.LastName,
                Number=user.Number,
                Password=user.Password,
                Adress=user.Adress,
                EntryDate=user.EntryDate,
                Balance=user.Balance
            };
        }
        public User ConvertDtoToUser(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                FirstName = userDto.FirstName,
                SecondName = userDto.SecondName,
                LastName = userDto.LastName,
                Number = userDto.Number,
                Password = userDto.Password,
                Adress = userDto.Adress,
                EntryDate = userDto.EntryDate,
                Balance = userDto.Balance,
                Status = userDto.Status,
                Access = userDto.Access
            };
        }

        public IEnumerable<UserDto> GetAllUserDto()
        {
            List<UserDto> userDtos = new List<UserDto>();
            List<User> users = data.Users.GetAllUsers().ToList();
            if (users!=null) {
                foreach (User user in users)
                {
                    userDtos.Add(ConvertUserToDto(user));
                }
            }
            return userDtos;
        }
        public UserDto GetUserDtoById(string id)
        {
            return ConvertUserToDto(data.Users.GetUserById(id));
        }
        public void DeleteUserById(string id)
        {
            data.Users.DeleteUserById(id);
        }
        public void SaveUserDto(UserDto userDtoToSave)
        {
            data.Users.SaveUser(ConvertDtoToUser(userDtoToSave));
        }
    }
}
