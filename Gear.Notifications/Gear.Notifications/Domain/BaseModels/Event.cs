using System;
using System.Collections.Generic;
using Gear.Notifications.Domain.Helpers;

namespace Gear.Notifications.Domain.BaseModels
{
    public class Event 
    {
        public Guid Id { get; set; }

        public string EventName { get; set; } = "No Name was provided for this event";

        public ICollection<NotificationEvent> NotificationEvents { get; set; }

        public HtmlEventMarkup HtmlEventMarkup { get; set; }

        public Guid HtmlEventMarkupId { get; set; }

        /// <summary>
        /// Parse this field to get the list of
        /// propagation for the event.
        /// </summary>
        public string PropagationTypes { get; set; }

        /// <summary>
        /// Parse this field to get the list of
        /// types this event belongs to.
        /// </summary>
        public string NotificationTypes { get; set; }
    }
}
