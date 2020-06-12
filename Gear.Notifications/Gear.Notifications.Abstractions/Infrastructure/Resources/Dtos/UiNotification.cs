using System;
using Gear.Notifications.Abstractions.Domain;

namespace Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos
{
    /// <summary>
    /// Notification that is used on the UI to alert the user.
    /// </summary>
    public class UiNotification
    {
        /// <summary>
        /// Notification Id
        /// </summary>
        public Guid Id { get; set; }

        public NotificationMessage Message { get; set; }
            = new NotificationMessage();

        public string NotificationType { get; set; }
            = "Action";

        /// <summary>
        /// The id of the entity the notifications
        /// will be grouped against
        /// </summary>
        public Guid? EntityGroupId { get; set; }
            = Guid.NewGuid();

        /// <summary>
        /// The name of group the notification belongs to
        /// </summary>
        public string EntityGroupName { get; set; }
            = "";

        /// <summary>
        /// Redirect data
        /// </summary>
        public MessageRedirectAction MessageRedirectAction { get; set; } = 
            new MessageRedirectAction();

    }

    /// <summary>
    /// Notification Message 
    /// that will keep increasing the number of fields.
    /// </summary>
    public class NotificationMessage
    {
        public string Title { get; set; } = "";

        public string Body { get; set; } = "";
    }

}
