using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;

namespace Gear.Notifications.Abstractions.Infrastructure.Interfaces
{
    public interface INotificationService
    {

        /// <summary>
        /// Checks if the event has any associated notification profiles,
        /// used to as a pre check for GetUsers.
        /// </summary>
        /// <returns></returns>
        bool CheckIfEventProfileExists(string eventName);

        /// <summary>
        /// Send notification on email.
        /// </summary>
        /// <remarks>
        /// Include event name for specific UI notifications
        /// </remarks>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAsync(Message message);

        /// <summary>
        /// Gets the users who should receive notifications for specific event.
        /// <remarks>
        /// Should be only if CheckIfEventProfileExists() returns true.
        /// </remarks>
        /// <exception>
        /// Can throw NotFoundException if called before CheckIfEventProfileExists().
        /// </exception>
        /// </summary>
        /// <returns></returns>
        Task<IList<string>> GetUsers(string eventName, ICollection<IApplicationUser> applicationUsers);

        /// <summary>
        /// Get a list of all user notifications.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task<IList<UiNotification>> GetUserNotifications(string userEmail);

        /// <summary>
        /// Set specific notification as read.
        /// </summary>
        /// <param name="notificationId"></param>
        Task MarkNotificationAsRead(Guid notificationId);

        /// <summary>
        /// Set user notifications as read.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        Task MarkAllUserNotificationsAsRead(string userEmail);
    }
}
