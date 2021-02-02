﻿using Taskr.Domain;
using Taskr.Dtos.Profile;
using Taskr.Infrastructure.UserNotification;

namespace Taskr.MappingProfiles.UserNotifications
{
    public class NotificationsProfile : AutoMapper.Profile
    {
        public NotificationsProfile()
        {
            CreateMap<AppUserNotification, UserNotificationDto>();
        }
    }
}