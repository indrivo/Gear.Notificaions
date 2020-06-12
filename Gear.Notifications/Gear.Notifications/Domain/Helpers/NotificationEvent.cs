using System;
using Gear.Notifications.Domain.BaseModels;

namespace Gear.Notifications.Domain.Helpers
{
    public class NotificationEvent 
    {
        public Event Event { get; set; }

        public Guid EventId { get; set; }

        public NotificationProfile NotificationProfile { get; set; }

        public Guid NotificationProfileId { get; set; }

    }
}
