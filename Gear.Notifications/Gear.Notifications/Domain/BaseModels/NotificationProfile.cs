using System;
using System.Collections.Generic;
using Gear.Notifications.Domain.Helpers;

namespace Gear.Notifications.Domain.BaseModels
{
    public class NotificationProfile
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "No name was provided for the profile";

        public ICollection<NotificationUser> NotificationUsers { get; set; }

        public ICollection<NotificationEvent> NotificationEvents { get; set; }
    }
}
