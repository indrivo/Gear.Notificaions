using System;
using Gear.Notifications.Abstractions.Domain;

namespace Gear.Notifications.Domain.BaseModels
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string Users { get; set; }

        public string Message { get; set; }

        public NotificationType NotificationType { get; set; }

        public string RedirectAction { get; set; }

        public Guid? PrimaryEntityGroup { get; set; }

        public string PrimaryEntityGroupName { get; set; }
    }
}
