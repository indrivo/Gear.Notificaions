using System.Collections.Generic;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;

namespace Gear.Notifications.Abstractions.Infrastructure.Hubs
{
    /// <summary>
    /// Interface defining all the methods the hub has
    /// meaning the server can send
    /// </summary>
    public interface IGearHub
    {
        /// <summary>
        /// Send Notification to specific user
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendNotification(IList<string> emails, UiNotification message);

        /// <summary>
        /// Send chat message to all users
        /// </summary>
        /// <param name="message"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task SendMessage(string email, string message);
    }
}
