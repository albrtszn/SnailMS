using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface IRoleRepo
    {
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(string id);
        void SaveRole(Role roleToSave);
        void DeleteRoleById(string id);
    }
}
