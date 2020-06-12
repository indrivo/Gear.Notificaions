using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Hubs;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Gear.Notifications.Infrastructure.Hubs
{
    /// <summary>
    /// Hub for sending notifications to the application
    /// </summary>
    public class GearHub : Hub<IGearClient>, IGearHub
    {
        private readonly IHubContext<GearHub, IGearClient> _hubContext;

        private static ConcurrentDictionary<string, string> 
            Connections = new ConcurrentDictionary<string, string>();

        public GearHub(IHubContext<GearHub, IGearClient> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Creates connection with the client during handshake
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            if (!Connections.ContainsKey(Context.ConnectionId) && Context.User.Identity.IsAuthenticated)
            {
                Connections.TryAdd(this.Context.User.Identity.Name, Context.ConnectionId);
            }

            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Broadcast message on the hub
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string email, string message)
        {
            await _hubContext.Clients.All.ReceiveMessage(email, message);
        }

        /// <summary>
        /// Send Notification to specific user
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendNotification(IList<string> emails, UiNotification message)
        {
            var connections = new List<string>();
            foreach (var email in emails)
            {
                if (!Connections.TryGetValue(email.Trim(), out var connectionToSendMessage)) continue;
                if (!string.IsNullOrWhiteSpace(connectionToSendMessage))
                {
                    connections.Add(connectionToSendMessage);
                }
            }
            await _hubContext.Clients.Clients(connections).ReceiveNotification(message);
        }
    }
}