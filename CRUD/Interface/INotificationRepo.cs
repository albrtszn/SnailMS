using DataBase.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Interface
{
    public interface INotificationRepo
    {
        IEnumerable<Notification> GetAllNotifications();
        Notification GetNotificationById(int id);
        void SaveNotification(Notification notificationToSave);
        void DeleteNotificationById(int id);
    }
}
