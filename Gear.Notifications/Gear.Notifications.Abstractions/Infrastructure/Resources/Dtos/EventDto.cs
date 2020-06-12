using System;
using System.Collections.Generic;
using Gear.Notifications.Abstractions.Domain;

namespace Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos
{
    public class EventDto
    {
        public Guid Id { get; set; }

        public string EventName { get; set; }

        public IList<NotificationType> NotificationTypes { get; set; }

        public IList<PropagationType> PropagationTypes { get; set; }
    }
}
