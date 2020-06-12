using System;
using Gear.Notifications.Domain.BaseModels;

namespace Gear.Notifications.Domain.Helpers
{
    public class NotificationUser
    {
        public Guid NotificationProfileId { get; set; }

        public NotificationProfile NotificationProfile { get; set; }

        public Guid UserId { get; set; }
    }
}
