using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface ICallRepo
    {
        IEnumerable<Call> GetAllCalls();
        Call GetCallById(string id);
        void SaveCall(Call callToSave);
        void DeleteCallById(string id);
    }
}
