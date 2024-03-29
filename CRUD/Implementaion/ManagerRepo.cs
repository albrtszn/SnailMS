﻿using CRUD.Interface;
using DataBase.Entity;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Implementaion
{
    public class ManagerRepo :IManagerRepo
    {
        private EFDBContext context;
        public ManagerRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteManagerById(string id)
        {
            Manager? managerToDelete = GetAllManagers().FirstOrDefault(x => x.Id.Equals(id));
            if (managerToDelete != null || !string.IsNullOrEmpty(managerToDelete.Id))
            {
                context.Managers.Remove(managerToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Manager> GetAllManagers()
        {
            IEnumerable<Manager> managers = context.Managers;
            return managers;
        }

        public Manager GetManagerById(string id)
        {
            Manager manager = context.Managers.FirstOrDefault(x => x.Id.Equals(id));
            return manager;
        }

        public void SaveManager(Manager managerToSave)
        {
            if (GetManagerById(managerToSave.Id) != null)
            {
                DeleteManagerById(managerToSave.Id);
            }
            context.Managers.Add(managerToSave);
            context.SaveChanges();
        }
    }
}
