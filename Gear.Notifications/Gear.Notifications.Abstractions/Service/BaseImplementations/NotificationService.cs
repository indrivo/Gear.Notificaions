using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Settings;

namespace Gear.Notifications.Abstractions.Service.BaseImplementations
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationServiceSettings _settings;

        public NotificationService(NotificationServiceSettings settings)
        {
            _settings = settings;
        }

        public async Task SendAsync(Message message)
        {
            #region SMTP

            var client = new SmtpClient
            {
                Host = _settings.Host,
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.UserName, _settings.UserPass),
                EnableSsl = _settings.EnableSsl
            };

            var mailMessage = new MailMessage()
            {
                Subject = message.Subject,
                Body = message.Body.Replace("\r\n \n", "<br />"),
                From = new MailAddress(_settings.UserName)
            };
            foreach (var address in message.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                mailMessage.To.Add(address);
            }

            foreach (var att in message.Attachments)
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(att.Content), att.Filename, att.Type));
            }

            await client.SendMailAsync(mailMessage);

            #endregion
        }

        public Task<IList<string>> GetUsers(string eventName, ICollection<IApplicationUser> applicationUsers)
        {
            IList<string> result = new List<string>();
            return Task.FromResult(result);
        }

        public bool CheckIfEventProfileExists(string eventName)
        {
            return false;
        }

        public Task<IList<UiNotification>> GetUserNotifications(Guid userId)
        {
            return Task.FromResult<IList<UiNotification>>(new List<UiNotification>());
        }

        public Task MarkNotificationAsRead(Guid notificationId)
        {
            return Task.CompletedTask;
        }

        public Task MarkAllUserNotificationsAsRead(Guid userId)
        {
            return Task.CompletedTask;
        }

        public Task<IList<UiNotification>> GetUserNotifications(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Task MarkAllUserNotificationsAsRead(string userEmail)
        {
            throw new NotImplementedException();
        }
    }
}
