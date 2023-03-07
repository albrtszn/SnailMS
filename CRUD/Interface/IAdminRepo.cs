using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface IAdminRepo
    {
        IEnumerable<Admin> GetAllAdmins();
        Admin GetAdminById(string id);
        void SaveAdmin(Admin adminToSave);
        void DeleteAdminById(string id);
    }
}
