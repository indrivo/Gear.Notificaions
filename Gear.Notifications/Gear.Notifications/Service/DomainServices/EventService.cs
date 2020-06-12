using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Common.Exceptions;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using Gear.Notifications.Domain.BaseModels;
using Gear.Notifications.Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gear.Notifications.Service.DomainServices
{
    public class EventService : IEventService
    {
        private readonly GearNotificationsContext _notificationsContext;

        public EventService(GearNotificationsContext notificationsContext)
        {
            _notificationsContext = notificationsContext;
        }

        public virtual async Task CreateEvent(string eventName)
        {
            if(_notificationsContext.Events.Any(x => x.EventName == eventName)) return;

            await _notificationsContext.AddAsync(new Event()
            {
                Id = Guid.NewGuid(),
                EventName = eventName,
            });
            await _notificationsContext.SaveChangesAsync();
        }

        /// <summary>
        /// Can be done by sending the whole assembly
        /// to it instead of string but i don't know how practical that is
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public virtual async Task ImportAndCreateEvents(Assembly assemblyName)
        {
            var importEvents = assemblyName.GetTypes()
                .Where(typeof(INotification).IsAssignableFrom)
                .Select(x => x.Name).Except(_notificationsContext.Events
                    .Select(x => x.EventName))
                .Select(@event => new Event()
                    { Id = Guid.NewGuid(),
                        EventName = @event,
                        NotificationTypes = JsonConvert.SerializeObject(new List<NotificationType>() { NotificationType.Action }),
                        PropagationTypes = JsonConvert.SerializeObject(new List<PropagationType>() { PropagationType.Email, PropagationType.Application })
                    }).ToList();

            await _notificationsContext.AddRangeAsync(importEvents);
            await _notificationsContext.SaveChangesAsync();
        }

        /// <summary>
        /// Same as above mentioned regarding the assembly
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="Interface"></param>
        /// <returns></returns>
        public virtual async Task ImportAndCreateEvents(Assembly assemblyName, Type Interface)
        {
            var events = assemblyName.GetTypes().Where(Interface.IsAssignableFrom).ToList();

            var eventsList = events.Select(@event => new Event()
                { Id = Guid.NewGuid(),
                    EventName = @event.Name,
                    NotificationTypes = JsonConvert.SerializeObject(new List<NotificationType>(){NotificationType.Action}),
                    PropagationTypes = JsonConvert.SerializeObject(new List<PropagationType>(){PropagationType.Email,PropagationType.Application})
                }).ToList();

            await _notificationsContext.AddRangeAsync(eventsList);
            await _notificationsContext.SaveChangesAsync();
        }

        public virtual async Task AddRange(ICollection<string> eventNames)
        {
            var eventList = eventNames.Except(_notificationsContext.Events
                .Select(x => x.EventName))
                .Select(eventName => new Event()
                    { Id = Guid.NewGuid(), EventName = eventName}).ToList();

            await _notificationsContext.AddRangeAsync(eventList);
            await _notificationsContext.SaveChangesAsync();
        }

        public virtual async Task<IDictionary<Guid, string>> GetAllEventsNames()
        {
            if (_notificationsContext.Events.Any())
                return await _notificationsContext.Events.OrderBy(s => s.EventName)
                    .ToDictionaryAsync(x => x.Id, x => x.EventName);
            return new Dictionary<Guid, string>();
        }

        public virtual async Task<string> GetEventName(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty) throw new IdNullOrEmptyException();
            var entity = await _notificationsContext.Events.FindAsync(id);
            if (entity == null) throw new NotFoundException(typeof(Event).Name, $"{id}");
            return entity.EventName;
        }

        public async Task<EventDto> GetEventModel(Guid? id)
        {
            if (id == null || id == Guid.Empty) throw new IdNullOrEmptyException();

            var dbEvent = await _notificationsContext.Events.FirstOrDefaultAsync(x => x.Id == id);

            if (dbEvent == null) throw new NotFoundException(typeof(Event).Name, id.ToString());

            return new EventDto()
            {
                Id = id.Value,
                EventName = dbEvent.EventName,
                PropagationTypes = JsonConvert.DeserializeObject<List<PropagationType>>(dbEvent.PropagationTypes),
                NotificationTypes = JsonConvert.DeserializeObject<List<NotificationType>>(dbEvent.NotificationTypes)
            };
        }

        public virtual async Task DeleteEvent(Guid? id)
        {
            if (!id.HasValue || id != Guid.Empty) throw new IdNullOrEmptyException();

            var target = await _notificationsContext.Events.FindAsync(id);

            if (target == null) throw new NotFoundException(typeof(Event).Name, id.ToString());
            _notificationsContext.Events.Remove(target);
            await _notificationsContext.SaveChangesAsync();
        }

        public virtual async Task UpdateEvent(EventDto eventModel)
        {
            if (eventModel.Id == Guid.Empty) throw new IdNullOrEmptyException();

            var target = await _notificationsContext.Events.FindAsync(eventModel.Id);

            if (target == null) throw new NotFoundException(typeof(Event).Name, eventModel.Id.ToString());

            target.EventName = eventModel.EventName;
            target.NotificationTypes = JsonConvert.SerializeObject(eventModel.NotificationTypes);
            target.PropagationTypes = JsonConvert.SerializeObject(eventModel.PropagationTypes);
            _notificationsContext.Update(target);
            await _notificationsContext.SaveChangesAsync();
        }

    }
}
