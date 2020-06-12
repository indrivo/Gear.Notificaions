using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Common.Exceptions;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Settings;
using Gear.Notifications.Domain.BaseModels;
using Gear.Notifications.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Attachment = SendGrid.Helpers.Mail.Attachment;

namespace Gear.Notifications.Service.NotificationServices
{
    public class SendGridNotificationService : INotificationService
    {
        private readonly GearNotificationsContext _notificationsContext;
        private readonly IOptions<SendGridNotificationServiceSettings> _settings;

        public SendGridNotificationService(GearNotificationsContext notificationsContext,
            IOptions<SendGridNotificationServiceSettings> settings)
        {
            _notificationsContext = notificationsContext;
            _settings = settings;
        }

        public virtual async Task SendAsync(Message message)
        {
            #region SendGridClient

            var client = new SendGridClient(_settings.Value.ApiKey);

            var from = new EmailAddress(_settings.Value.UserName);
            var subject = message.Subject;
            var htmlContent = message.Body.Replace("\r\n", "<br />").Replace("\n", "<br />");

            var toAdresses = message.To.Split(new[] {";"},
                StringSplitOptions.RemoveEmptyEntries).Select(address => new EmailAddress(address)).ToList();

            var mailMessage = MailHelper.CreateSingleEmailToMultipleRecipients(from, toAdresses, 
                subject,
                message.Body,
                htmlContent);

            if (message.Attachments != null)
            {
                mailMessage.Attachments = new List<Attachment>();
                foreach (var att in message.Attachments)
                {
                    mailMessage.Attachments.Add(new Attachment() { Content = Convert.ToBase64String(att.Content), Filename = att.Filename, Type = att.Type });
                }
            }

            await client.SendEmailAsync(mailMessage);

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

        public bool CheckIfEventProfileExists(string eventName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UiNotification>> GetUserNotifications(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task MarkNotificationAsRead(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task MarkAllUserNotificationsAsRead(string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
