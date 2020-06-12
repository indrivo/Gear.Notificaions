using System;

namespace Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos
{
    public class HtmlEventMarkupModelDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Subtitle { get; set; }

        public string ChangesMarkup { get; set; }

        public Guid EventId { get; set; }
    }
}
