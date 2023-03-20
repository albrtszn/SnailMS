using DataBase.Entity;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace CRUD.Implementaion
{
    public class UserRepo : IUserRepo
    {
        private EFDBContext context;
        public UserRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteUserById(string id)
        {
            User? userToDelete = GetAllUsers().FirstOrDefault(x => x.Id.Equals(id));
            if (userToDelete != null || !string.IsNullOrEmpty(userToDelete.Id))
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            IEnumerable<User> users = context.Users.ToList();
            users.ToList().ForEach(x=> x.Password=Encoding.UTF8.GetString(System.Convert.FromBase64String(x.Password)));
            return users;   
        }

        public User? GetUserById(string id)
        {
            User? user = context.Users.ToList().Find(x => x.Id.Equals(id));
            if (user != null)
            {
                user.Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(user.Password));
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
            userToSave.Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userToSave.Password));
            context.Users.Add(userToSave);
            context.SaveChanges();
        }
    }
}
