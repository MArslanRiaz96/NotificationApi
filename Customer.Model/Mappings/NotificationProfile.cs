using AutoMapper;
using Customer.Data.Models;
using Customer.Model.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationsModel>();
            CreateMap<NotificationsModel, Notification>();
            CreateMap<PushNotificationModel, Notification>().ReverseMap();
        }
    }
}
