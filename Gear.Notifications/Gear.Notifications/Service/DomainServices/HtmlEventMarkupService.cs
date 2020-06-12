using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Common.Exceptions;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using Gear.Notifications.Domain.BaseModels;
using Gear.Notifications.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Gear.Notifications.Service.DomainServices
{
    public class HtmlEventMarkupService : IHtmlEventMarkupService
    {
        private readonly GearNotificationsContext _notificationsContext;

        public HtmlEventMarkupService(GearNotificationsContext notificationsContext)
        {
            _notificationsContext = notificationsContext;
        }

        public virtual async Task CreateHtmlEventMarkup(HtmlEventMarkupModelDto model)
        {
            var entity = new HtmlEventMarkup()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Subject = model.Subject,
                Subtitle = model.Subtitle,
                ChangesMarkup = model.ChangesMarkup,
                EventId = model.EventId
            };
            await _notificationsContext.HtmlEventMarkups.AddAsync(entity);
            var eventEntity = await _notificationsContext.Events.FindAsync(entity.EventId);
            if (eventEntity == null) throw new NotFoundException(typeof(Event).Name, entity.EventId.ToString());
            eventEntity.HtmlEventMarkupId = entity.Id;

            _notificationsContext.Events.Update(eventEntity);
            await _notificationsContext.SaveChangesAsync();
        }

        public virtual async Task<IDictionary<Guid, string>> GetAllEventMarkUps()
        {
            return await _notificationsContext.HtmlEventMarkups.ToDictionaryAsync(x => x.Id, x => x.Name);
        }

        public virtual async Task<HtmlEventMarkupModelDto> GetHtmlEventMarkup(Guid? markupId)
        {
            if (!markupId.HasValue || markupId == Guid.Empty) throw new IdNullOrEmptyException();

            var model = await _notificationsContext.HtmlEventMarkups.FindAsync(markupId);

            if(model == null) throw new NotFoundException(typeof(HtmlEventMarkup).Name,markupId.ToString());

            var entity = new HtmlEventMarkupModelDto()
            {
                Id = model.Id,
                Subject = model.Subject,
                Name = model.Name,
                EventId = model.EventId,
                Subtitle = model.Subtitle,
                ChangesMarkup = model.ChangesMarkup
            };
            return entity;
        }

        public virtual async Task<HtmlEventMarkupModelDto> GetHtmlEventMarkupByEvent(Guid? eventId)
        {
            if (!eventId.HasValue || eventId == Guid.Empty) throw new IdNullOrEmptyException();

            var model = await _notificationsContext.HtmlEventMarkups.FirstAsync(x => x.EventId == eventId);

            if (model == null) throw new NotFoundException(typeof(HtmlEventMarkup).Name, eventId.ToString());

            var entity = new HtmlEventMarkupModelDto()
            {
                Id = model.Id,
                Subject = model.Subject,
                Name = model.Name,
                EventId = model.EventId,
                Subtitle = model.Subtitle,
                ChangesMarkup = model.ChangesMarkup
            };
            return entity;
        }

        public virtual async Task UpdateHtmlEventMarkup(HtmlEventMarkupModelDto model)
        {
            var entity = await _notificationsContext.HtmlEventMarkups.FindAsync(model.Id);
            if (entity == null) throw new NotFoundException(typeof(HtmlEventMarkup).Name, model.Id.ToString());

            entity.Name = model.Name;
            entity.Subject = model.Subject;
            entity.Subtitle = model.Subtitle;
            entity.ChangesMarkup = model.ChangesMarkup;
            _notificationsContext.Update(entity);
            await _notificationsContext.SaveChangesAsync();
        }

        public virtual async Task DeleteHtmlEventMarkup(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty) throw new IdNullOrEmptyException();

            var entity = await _notificationsContext.HtmlEventMarkups.FindAsync(id);

            if (entity == null) throw new NotFoundException(typeof(HtmlEventMarkup).Name, id.ToString());

            _notificationsContext.HtmlEventMarkups.Remove(entity);
            await _notificationsContext.SaveChangesAsync();
        }

        public virtual async Task AssignMarkupToEvent(Guid? eventId, Guid? markupId)
        {
            if(!eventId.HasValue || eventId != Guid.Empty 
                                 || !markupId.HasValue || markupId == Guid.Empty)
                throw new IdNullOrEmptyException();

            var target = await _notificationsContext.Events.FindAsync(eventId);

            if(target == null) throw new NotFoundException(typeof(Event).Name, eventId.ToString());

            target.HtmlEventMarkupId = markupId.Value;

            _notificationsContext.Update(target);
            await _notificationsContext.SaveChangesAsync();
        }
    }
}
