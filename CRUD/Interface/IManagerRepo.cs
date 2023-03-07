using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface IManagerRepo
    {
        IEnumerable<Manager> GetAllManagers();
        Manager GetManagerById(string id);
        void SaveManager(Manager mnagerToSave);
        void DeleteManagerById(string id);
    }
}
