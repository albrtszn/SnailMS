using CRUD;
using DataBase.Entity;
using SnailMS.Models;
using System.Text;

namespace SnailMS.Service
{
    public class UserService
    {
        private Data data;
        public UserService(Data _data)
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
        public UserDto? ConvertUserToDto(User user)
        {
            return new UserDto 
            { 
                Id=user.Id, 
                FirstName=user.FirstName, 
                SecondName=user.SecondName,
                LastName=user.LastName,
                Number=user.Number,
                Password= Base64Decode(user.Password),
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
                Password = Base64Encode(userDto.Password),
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
                Console.WriteLine($"get user: {user.Id}");
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
