using System;
using System.Collections.Generic;

namespace Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos
{
    public class NotificationProfileModelDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// List of users who should receive notifications
        /// if any event inside Events is triggered.
        /// </summary>
        public ICollection<Guid> Users { get; set; }

        /// <summary>
        /// List of events the users are subscribed to.
        /// </summary>
        public ICollection<Guid> Events { get; set; }
    }
}
