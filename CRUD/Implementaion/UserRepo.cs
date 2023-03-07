using DataBase.Entity;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.Interface;

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
            return context.Users.ToList();
        }

        public User GetUserById(string id)
        {
            return context.Users.FirstOrDefault(x => x.Id.Equals(id));
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
