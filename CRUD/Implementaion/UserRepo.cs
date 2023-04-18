using DataBase.Entity;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.Interface;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Implementaion
{
    public class UserRepo : IUserRepo
    {
        private EFDBContext context;
        public UserRepo(EFDBContext _context)
        {
            this.context = _context;
        }
        //convert logic
        // Base64
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

        //
        public void DeleteUserById(string id)
        {
            User? userToDelete = GetAllUsers().FirstOrDefault(x => x.Id.Equals(id));
            userToDelete.Password = Base64Encode(userToDelete.Password);
            if (userToDelete != null || !string.IsNullOrEmpty(userToDelete.Id))
            {
                context.Users.Remove(userToDelete);
                //context.Entry(userToDelete).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> users = new List<User>(context.Users.ToList());
            //users.ToList().ForEach(x=> x.Password = Base64Decode(x.Password));
            return users;   
        }

        public User? GetUserById(string id)
        {
            User? user = context.Users.ToList().Find(x => x.Id.Equals(id));
            if (user != null)
            {
                user.Password = Base64Decode(user.Password);
                return user;
            }
            return null;
        }

        public void SaveUser(User userToSave)
        {
            if (GetUserById(userToSave.Id) != null)
            {
                DeleteUserById(userToSave.Id);
            }
            userToSave.Password = Base64Encode(userToSave.Password);
            context.Users.Add(userToSave);
            context.SaveChanges();
        }
    }
}
