using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
using Newtonsoft.Json;
using Personalization = Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos.Personalization;

namespace Gear.Notifications.Service.NotificationServices
{
    public class RensonNotificationService : INotificationService
    {
        private readonly GearNotificationsContext _notificationsContext;
        private readonly IOptions<RensonNotificationServiceSettings> _settings;

        public RensonNotificationService(GearNotificationsContext notificationsContext,
            IOptions<RensonNotificationServiceSettings> settings)
        {
            _notificationsContext = notificationsContext;
            _settings = settings;
        }

        public virtual async Task SendAsync(Message message)
        {
            #region RensonNotificationClient

            Token token = null;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var addParams = new Dictionary<string, string>
                {
                   { "client_id", _settings.Value.ClientId},
                   { "client_secret", _settings.Value.ClientSecret },
                   { "scope", _settings.Value.Scope},
                   { "grant_type", "client_credentials" }
                };

                var content = new FormUrlEncodedContent(addParams);

                var responseAAD = await client.PostAsync(_settings.Value.AadURL, content);

                var responseStringADD = await responseAAD.Content.ReadAsStringAsync();

                token = JsonConvert.DeserializeObject<Token>(responseStringADD);
            }

            if (token != null && !string.IsNullOrEmpty(token.access_token) && !string.IsNullOrEmpty(message.To))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);

                    var toAdresses = new List<RensonMailAddress>();

                    foreach (var address in message.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        toAdresses.Add(new RensonMailAddress { email = address, name = string.Empty });
                    }

                    var values = new RensonNotificationModel();
                    values.from = new RensonMailAddress { email = _settings.Value.UserName, name = string.Empty };
                    values.personalizations = new List<Personalization>();
                    values.personalizations.Add(new Personalization
                    {
                        to = toAdresses,
                        subject = message.Subject,
                        sendAt = DateTime.Now,
                        dynamicData = new
                        {
                            firstname = message.From,
                            linkToZip = message.Body,
                            linkName = message.Subject
                        }
                    });
                    values.templateId = _settings.Value.TemplateId;
                    values.subject = "";
                    values.sendAt = DateTime.Now;
                    values.attachments = new List<RensonAttachment>();


                    client.BaseAddress = new Uri(_settings.Value.ApiURL);

                    var inputContentRensonAPI = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json");

                    await client.PostAsync("/mail/send", inputContentRensonAPI);

                }
            }
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

        public Task<IList<UiNotification>> GetUserNotifications(string userEmail)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfEventProfileExists(string eventName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UiNotification>> GetUserNotifications(Guid userId)
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
