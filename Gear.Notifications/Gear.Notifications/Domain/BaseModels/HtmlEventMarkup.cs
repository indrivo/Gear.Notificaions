using System;

namespace Gear.Notifications.Domain.BaseModels
{
    public class HtmlEventMarkup 
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = "No name was given";

        public string Subject { get; set; } = "You have received a new notification";

        public string Subtitle { get; set; } = " ";

        public string ChangesMarkup { get; set; } = " ";

        public Guid EventId { get; set; }

        public Event Event { get; set; }
    }
}
