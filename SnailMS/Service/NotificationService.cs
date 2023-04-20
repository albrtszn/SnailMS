using CRUD;
using DataBase.Entity;
using SnailMS.Models;

namespace SnailMS.Service
{
    public class NotificationService
    {
        private Data data;
        public NotificationService(Data _data)
        {
            data = _data;
        }

        public NotificationDto ConvertNotificationToDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Message = notification.Message
            };
        }
        public Notification ConvertDtoToNotification(NotificationDto notificationDto)
        {
            return new Notification
            {
                UserId = notificationDto.UserId,
                Message = notificationDto.Message
            };
        }

        public IEnumerable<NotificationDto> GetAllNotificationDto()
        {
            List<NotificationDto> notificationDtos = new List<NotificationDto>();
            List<Notification> notifications = data.Notifications.GetAllNotifications().ToList();
            if (notificationDtos != null)
            {
                foreach (Notification note in notifications)
                {
                    notificationDtos.Add(ConvertNotificationToDto(note));
                }
            }
            return notificationDtos;
        }
        public NotificationDto GetNotificationDtoById(int id)
        {
            return ConvertNotificationToDto(data.Notifications.GetNotificationById(id));
        }
        public void DeleteNotificationById(int id)
        {
            data.Notifications.DeleteNotificationById(id);
        }
        public void SaveNotificationDto(NotificationDto CallDtoToSave)
        {
            data.Notifications.SaveNotification(ConvertDtoToNotification(CallDtoToSave));
        }
    }
}
