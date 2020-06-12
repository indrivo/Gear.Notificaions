using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Common.Exceptions;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Hubs;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Settings;
using Gear.Notifications.Domain.BaseModels;
using Gear.Notifications.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gear.Notifications.Service.NotificationServices
{
    public class GearNotificationsService : INotificationService
    {
        private readonly GearNotificationsContext _notificationsContext;
        private readonly IGearHub _hub;
        private readonly IOptions<NotificationServiceSettings> _settings;

        public GearNotificationsService(GearNotificationsContext notificationsContext,
            IOptions<NotificationServiceSettings> settings, IGearHub hub)
        {
            _notificationsContext = notificationsContext;
            _settings = settings;
            _hub = hub;
        }


        public bool CheckIfEventProfileExists(string eventName)
        {
            return _notificationsContext.NotificationEvents.Any()
                && _notificationsContext.NotificationEvents.Include(x => x.Event).Any(x => x.Event.EventName == eventName);
        }


        public virtual async Task SendAsync(Message message)
        {
            #region Smtp

            if (!CheckIfEventExists(message.EventName))
                throw new NotFoundException("Entity not found", message.EventName);

            var type = await GetEventPropagation(message.EventName);

            if (type.Contains(PropagationType.Email)) await SendEmail(message);

            if (type.Contains(PropagationType.Application)) await SendApplicationNotification(message);

            #endregion
        }


        /// <summary>
        /// Get the list of all the users who are listening to a specific event
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="applicationUsers"></param>
        /// <returns></returns>
        public virtual async Task<IList<string>> GetUsers(string eventName, ICollection<IApplicationUser> applicationUsers)
        {
            if (!applicationUsers.Any())
            {
                return new List<string>();
            }
            var triggeredEvent = await _notificationsContext.Events.FirstAsync(x =>
                x.EventName.ToLowerInvariant().Equals(eventName.ToLowerInvariant()));

            if (triggeredEvent == null) throw new NotFoundException(typeof(Event).Name, eventName);

            var notificationEvents = await _notificationsContext.NotificationEvents.Where(x => x.EventId == triggeredEvent.Id)
                .Include(x => x.NotificationProfile)
                .ThenInclude(x => x.NotificationUsers).ToListAsync();

            var userIdList = new List<Guid>();

            foreach (var notificationEvent in notificationEvents)
            {
                userIdList.AddRange(notificationEvent.NotificationProfile.NotificationUsers.Select(x => x.UserId));
            }

            var emails = applicationUsers.Where(x => userIdList.Contains(x.Id)).Select(x => x.Email).ToList();

            return emails;
        }


        public async Task<IList<UiNotification>> GetUserNotifications(string userEmail)
        {
            return await _notificationsContext.Notifications
                .Where(x => x.Users.ToLowerInvariant().Contains(userEmail.ToLowerInvariant())).Select(x => new UiNotification
                {
                    Id = x.Id,
                    Message = JsonConvert.DeserializeObject<NotificationMessage>(x.Message),
                    NotificationType = x.NotificationType.ToString()
                }).ToListAsync();
        }


        public async Task MarkNotificationAsRead(Guid notificationId)
        {
            var notification = await _notificationsContext.Notifications.FindAsync(notificationId);
            _notificationsContext.Remove(notification);
            await _notificationsContext.SaveChangesAsync();
        }


        public async Task MarkAllUserNotificationsAsRead(string userEmail)
        {
            var notificationList = await _notificationsContext.Notifications
                .Where(x => x.Users.ToLowerInvariant().Contains(userEmail.ToLowerInvariant())).ToListAsync();
            _notificationsContext.RemoveRange(notificationList);
            await _notificationsContext.SaveChangesAsync();
        }


        /// <summary>
        /// Checks if there is any event with the
        /// corresponding name in the database.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private bool CheckIfEventExists(string eventName)
        {
            if (eventName == "") return false;
            var eventTarget = _notificationsContext.Events.FirstOrDefault(x => x.EventName == eventName);
            return eventTarget != null;
        }


        /// <summary>
        /// Gets the types of the specific event.
        /// <remarks>
        /// Use it to save it to database
        /// </remarks>
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private async Task<IList<NotificationType>> GetNotificationTypesForEvent(string eventName)
        {
            var eventTarget = await _notificationsContext.Events.FirstOrDefaultAsync(x => x.EventName == eventName);
            return JsonConvert.DeserializeObject<List<NotificationType>>(eventTarget.NotificationTypes);
        }


        /// <summary>
        /// Get the propagation spaces of the event notification.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        private async Task<IList<PropagationType>> GetEventPropagation(string eventName)
        {
            var eventTarget = await _notificationsContext.Events.FirstOrDefaultAsync(x => x.EventName == eventName);
            return JsonConvert.DeserializeObject<List<PropagationType>>(eventTarget.PropagationTypes);
        }


        /// <summary>
        /// Send email notification
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendEmail(Message message)
        {
            var client = new SmtpClient
            {
                Host = _settings.Value.Host,
                Port = _settings.Value.Port,
                Credentials = new NetworkCredential(_settings.Value.UserName, _settings.Value.UserPass),
                EnableSsl = _settings.Value.EnableSsl
            };

            var mailMessage = new MailMessage()
            {
                Subject = message.Subject,
                Body = message.Body.Replace("\r\n \n", "<br />"),
                From = new MailAddress(_settings.Value.UserName)
            };

            foreach (var address in message.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                if(string.IsNullOrWhiteSpace(address))continue;
                mailMessage.To.Add(address);
            }

            if (message.Attachments != null)
            {
                foreach (var att in message.Attachments)
                {
                    mailMessage.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(att.Content), att.Filename, att.Type));
                }
            }

            if (mailMessage.To.Any())
            {
                await client.SendMailAsync(mailMessage);
            }

        }


        /// <summary>
        /// Send notification to the Application
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendApplicationNotification(Message message)
        {
            var notifications = new List<Notification>();

            foreach (var notificationType in await GetNotificationTypesForEvent(message.EventName))
            {
                notifications.Add(new Notification()
                {
                    Id = Guid.NewGuid(),
                    NotificationType = notificationType,
                    Message = JsonConvert.SerializeObject(new NotificationMessage()
                    {
                        Body = message.Body,
                        Title = message.Subject
                    }),
                    Users = message.To,
                    RedirectAction = JsonConvert.SerializeObject(message.MessageRedirectAction),
                    PrimaryEntityGroup = message.PrimaryEntityGroup,
                    PrimaryEntityGroupName = message.PrimaryEntityGroupName
                });
            }

            await _notificationsContext.Notifications.AddRangeAsync(notifications);
            await _notificationsContext.SaveChangesAsync();

            foreach (var notification in notifications)
            {
                try
                {
                    await _hub.SendNotification(notification.Users.Split(';').ToList(), new UiNotification()
                    {
                        Id = notification.Id,
                        NotificationType = notification.NotificationType.ToString(),
                        Message = JsonConvert.DeserializeObject<NotificationMessage>(notification.Message),
                        EntityGroupId = notification.PrimaryEntityGroup,
                        EntityGroupName = notification.PrimaryEntityGroupName,
                        MessageRedirectAction = JsonConvert.DeserializeObject<MessageRedirectAction>(notification.RedirectAction)
                    });
                }
                catch (Exception e)
                {
                    //
                }
            }
        }

    }
}
