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
    public class CallRepo : ICallRepo
    {
        private EFDBContext context;
        public CallRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteCallById(string id)
        {
            Call? callToDelete = GetAllCalls().FirstOrDefault(x => x.Id.Equals(id));
            if (callToDelete != null || !string.IsNullOrEmpty(callToDelete.Id))
            {
                context.Calls.Remove(callToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Call> GetAllCalls()
        {
            return context.Calls.ToList();
        }

        public Call GetCallById(string id)
        {
            return context.Calls.FirstOrDefault(x => x.Id.Equals(id));
        }

        public void SaveCall(Call roleToSave)
        {
            if (GetCallById(roleToSave.Id) != null)
            {
                DeleteCallById(roleToSave.Id);
            }
            context.Calls.Add(roleToSave);
            context.SaveChanges();
        }
    }
}
