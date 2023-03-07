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
    public class DepartmentRepo : IDepartmentRepo
    {
        private EFDBContext context;
        public DepartmentRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteDepartmentById(string id)
        {
            Department? departmentToDelete = GetAllDepartments().FirstOrDefault(x => x.Name.Equals(id));
            if (departmentToDelete != null || !string.IsNullOrEmpty(departmentToDelete.Name))
            {
                context.Departments.Remove(departmentToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return context.Departments.ToList();
        }

        public Department GetDepartmentById(string id)
        {
            return context.Departments.FirstOrDefault(x => x.Name.Equals(id));
        }

        public void SaveDepartment(Department departmentToSave)
        {
            if (GetDepartmentById(departmentToSave.Name) != null)
            {
                DeleteDepartmentById(departmentToSave.Name);
            }
            context.Departments.Add(departmentToSave);
            context.SaveChanges();
        }
    }
}
