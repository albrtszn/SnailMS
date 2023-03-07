using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface IUserRoleRepo
    {
        IEnumerable<UserRole> GetAllUserRoles();
        UserRole GetUserRoleById(string id);
        void SaveUserRole(UserRole userRoleToSave);
        void DeleteUserRoleById(string id);
    }
}
