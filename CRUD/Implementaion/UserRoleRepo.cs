using DataBase.Entity;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.Interface;
using System.Security.Cryptography.X509Certificates;

namespace CRUD.Implementaion
{
    public class UserRoleRepo :IUserRoleRepo
    {
        private EFDBContext context;
        public UserRoleRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteUserRoleById(string id)
        {
            UserRole? userRoleToDelete = GetAllUserRoles().FirstOrDefault(x => x.UserId.Equals(id));
            if (userRoleToDelete!=null || !string.IsNullOrEmpty(userRoleToDelete.UserId))
            {
                context.UserRoles.Remove(userRoleToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<UserRole> GetAllUserRoles()
        {
            return context.UserRoles.ToList();
        }

        public UserRole GetUserRoleById(string id)
        {
            return context.UserRoles.FirstOrDefault(x => x.UserId.Equals(id));
        }

        public void SaveUserRole(UserRole userRoleToSave)
        {
            if (GetUserRoleById(userRoleToSave.UserId) != null)
            {
                DeleteUserRoleById(userRoleToSave.UserId);
            }
            context.UserRoles.Add(userRoleToSave);
            context.SaveChanges();
        }
    }
}
