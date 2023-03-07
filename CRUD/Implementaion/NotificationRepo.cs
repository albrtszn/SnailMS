using DataBase.Entity;
using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.Interface;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.Implementaion
{
    public class NotificationRepo : INotificationRepo
    {
        private EFDBContext context;
        public NotificationRepo(EFDBContext _context)
        {
            this.context = _context;
        }

        public void DeleteNotificationById(int id)
        {
            Notification? notificationToDelete = GetAllNotifications().FirstOrDefault(x => x.Id == id);
            if (notificationToDelete != null || notificationToDelete.Id != 0)
            {
                context.Notifications.Remove(notificationToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Notification> GetAllNotifications()
        {
            return context.Notifications.ToList();
        }

        public Notification GetNotificationById(int id)
        {
            return context.Notifications.FirstOrDefault(x => x.Id == id);
        }

        public void SaveNotification(Notification notificationToSave)
        {
            if (GetNotificationById(notificationToSave.Id) != null)
            {
                DeleteNotificationById(notificationToSave.Id);
            }
            context.Notifications.Add(notificationToSave);
            context.SaveChanges();
        }
    }
}
