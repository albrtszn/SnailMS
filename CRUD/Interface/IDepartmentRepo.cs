using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface IDepartmentRepo
    {
        IEnumerable<Department> GetAllDepartments();
        Department GetDepartmentById(string id);
        void SaveDepartment(Department departmentToSave);
        void DeleteDepartmentById(string id);
    }
}
