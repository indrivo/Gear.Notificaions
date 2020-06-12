using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;

namespace Gear.Notifications.Abstractions.Infrastructure.Hubs
{
    /// <summary>
    /// Strongly typed interface for the hub
    /// Defines methods that the client side should implement
    /// in order to work correctly with the hub
    /// </summary>
    public interface IGearClient
    {
        /// <summary>
        /// Method used to send a message to the hub
        /// and broadcast it to other user
        /// </summary>
        /// <param name="method"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        Task SendAsync(string method, object arg1, object arg2);

        /// <summary>
        /// Receive message from the hub
        /// </summary>
        /// <returns></returns>
        Task ReceiveMessage(object arg1, object arg2);

        /// <summary>
        /// Receive notification from the hub
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        Task ReceiveNotification(object notification);

    }
}
