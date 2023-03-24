using CRUD.Interface;
using DataBase.Entity;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Implementaion
{
    public class AdminRepo : IAdminRepo
    {
        private EFDBContext context;
        public AdminRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteAdminById(string id)
        {
            Admin? adminToDelete = GetAllAdmins().FirstOrDefault(x => x.Id.Equals(id));
            if (adminToDelete != null || !string.IsNullOrEmpty(adminToDelete.Id))
            {
                context.Admins.Remove(adminToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Admin> GetAllAdmins()
        {
            IEnumerable<Admin> admins = context.Admins;
            if (admins.Count() != 0)
            {
                admins.ToList().ForEach(x => x.Password = Encoding.UTF8.GetString(System.Convert.FromBase64String(x.Password)));
            }
            return admins.ToList();
        }

        public Admin GetAdminById(string id)
        {
            return context.Admins.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveAdmin(Admin adminToSave)
        {
            if (GetAdminById(adminToSave.Id) != null)
            {
                DeleteAdminById(adminToSave.Id);
            }
            context.Admins.Add(adminToSave);
            context.SaveChanges();
        }
    }
}
