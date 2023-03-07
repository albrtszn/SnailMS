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
    public class RoleRepo : IRoleRepo
    {
        private EFDBContext context;
        public RoleRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteRoleById(string id)
        {
            Role? roleToDelete = GetAllRoles().FirstOrDefault(x => x.Name.Equals(id));
            if (roleToDelete != null || !string.IsNullOrEmpty(roleToDelete.Name))
            {
                context.Roles.Remove(roleToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return context.Roles.ToList();
        }

        public Role GetRoleById(string id)
        {
            return context.Roles.FirstOrDefault(x => x.Name.Equals(id));
        }

        public void SaveRole(Role roleToSave)
        {
            if (GetRoleById(roleToSave.Name) != null)
            {
                DeleteRoleById(roleToSave.Name);
            }
            context.Roles.Add(roleToSave);
            context.SaveChanges();
        }
    }
}
