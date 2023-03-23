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
        
        public UserDto? ConvertUserToDto(User user)
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
                Balance=user.Balance,
                Status=user.Status,
                Access=user.Access, 
                Picture=user.Picture
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
                Access = userDto.Access,
                Picture = userDto.Picture
            };
        }

        public byte[]? GetUserAvatar(string userId)
        {
            UserDto? userDto = GetUserDtoById(userId);
            if (userDto != null)
            {
                Console.WriteLine(userDto.Picture);
                return userDto.Picture;
            }
            return null;
        }
        public byte[] IFormFileToByteArray(IFormFile file)
        {
            using var fileStream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            fileStream.Read(bytes, 0, (int)file.Length);

            return bytes;
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
        public UserDto? GetUserDtoById(string id)
        {
            User? user = data.Users.GetUserById(id);
            if (user != null)
            {
                Console.WriteLine($"get user: {user.Id}, {user.Picture.ToString}");
                return ConvertUserToDto(data.Users.GetUserById(id));
            }
            return null;
        }
        public void DeleteUserById(string id)
        {
            data.Users.DeleteUserById(id);
        }
        public void SaveUserDto(UserDto userDtoToSave)
        {
            if (GetUserDtoById(userDtoToSave.Id) != null)
            {
                DeleteUserById(userDtoToSave.Id);
            }
            data.Users.SaveUser(ConvertDtoToUser(userDtoToSave));
        }
    }
}
