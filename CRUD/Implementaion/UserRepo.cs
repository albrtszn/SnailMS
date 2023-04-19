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
            IEnumerable<User> users = new List<User>(context.Users.ToList());
            return users;   
        }

        public User? GetUserById(string id)
        {
            User? user = context.Users.ToList().Find(x => x.Id.Equals(id));
            if (user != null)
            {
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
            context.Users.Add(userToSave);
            context.SaveChanges();
        }
    }
}
