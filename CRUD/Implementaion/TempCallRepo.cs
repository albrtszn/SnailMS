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
    public class TempCallRepo : ITempCallRepo
    {
        private EFDBContext context;
        public TempCallRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteTempCallById(string id)
        {
            TempCall? tempCallToDelete = GetAllTempCalls().FirstOrDefault(x => x.Id.Equals(id));
            if (tempCallToDelete != null || !string.IsNullOrEmpty(tempCallToDelete.Id))
            {
                context.TempCalls.Remove(tempCallToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<TempCall> GetAllTempCalls()
        {
            return context.TempCalls.ToList();
        }

        public TempCall GetTempCallById(string id)
        {
            return context.TempCalls.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveTempCall(TempCall tempCallToSave)
        {
            if (GetTempCallById(tempCallToSave.Id) != null)
            {
                DeleteTempCallById(tempCallToSave.Id);
            }
            context.TempCalls.Add(tempCallToSave);
            context.SaveChanges();
        }
    }
}
