using System;
using System.Collections.Generic;

namespace Gear.Notifications.Abstractions.Domain
{
    public class Message
    {
        public string From { get; set; } = "";

        public string To { get; set; } = "";

        public string Subject { get; set; } = "";

        public string Body { get; set; } = "";

        public ICollection<Attachment> Attachments { get; set; }

        public string  EventName { get; set; }

        /// <summary>
        /// The Id of the entity
        /// that the notification
        /// will be grouped by
        /// </summary>
        public Guid? PrimaryEntityGroup { get; set; }

        /// <summary>
        /// The name of the group
        /// </summary>
        public string PrimaryEntityGroupName { get; set; }

        /// <summary>
        /// Redirect data
        /// </summary>
        public MessageRedirectAction MessageRedirectAction { get; set; }

    }
}
