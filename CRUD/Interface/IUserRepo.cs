using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface IUserRepo
    {
        IEnumerable<User> GetAllUsers();
        User? GetUserById(string id);
        void SaveUser(User userToSave);
        void DeleteUserById(string id);
    }
}
