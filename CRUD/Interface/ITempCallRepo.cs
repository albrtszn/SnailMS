using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface ITempCallRepo
    {
        IEnumerable<TempCall> GetAllTempCalls();
        TempCall GetTempCallById(string id);
        void SaveTempCall(TempCall tempCallToSave);
        void DeleteTempCallById(string id);
    }
}
