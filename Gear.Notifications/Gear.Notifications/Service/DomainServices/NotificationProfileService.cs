using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Common.Exceptions;
using Gear.Notifications.Abstractions.Domain;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;
using Gear.Notifications.Domain.BaseModels;
using Gear.Notifications.Domain.Helpers;
using Gear.Notifications.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gear.Notifications.Service.DomainServices
{
    /// <summary>
    /// TODO: introduce the check if exists
    /// </summary>
    public class NotificationProfileService : INotificationProfileService
    {
        private readonly GearNotificationsContext _notificationsContext;


        public NotificationProfileService(GearNotificationsContext notificationsContext)
        {
            _notificationsContext = notificationsContext;
        }


        public bool CheckIfProfileExists(Guid profileId)
        {
            return _notificationsContext.NotificationProfiles.Any() && _notificationsContext.NotificationProfiles.Any(x => x.Id == profileId);
        }


        public virtual async Task CreateNotificationProfile(NotificationProfileModelDto dto)
        {
            #region Profile

            var entity = new NotificationProfile()
            {
                Id = dto.Id,
                Name = dto.Name
            };
            await _notificationsContext.NotificationProfiles.AddAsync(entity);

            #endregion

            #region Users

            var users = dto.Users.Select(id => new NotificationUser()
            { UserId = id, NotificationProfileId = entity.Id }).ToList();
            await _notificationsContext.NotificationUsers.AddRangeAsync(users);

            #endregion

            #region profileEvents

            var profileEvents = dto.Events.Select(id => new NotificationEvent()
            { EventId = id, NotificationProfileId = entity.Id }).ToList();
            await _notificationsContext.NotificationEvents.AddRangeAsync(profileEvents);

            #endregion

            await _notificationsContext.SaveChangesAsync();
        }


        public virtual async Task<IDictionary<Guid, string>> GetAllNotificationProfiles()
        {
            return await _notificationsContext.NotificationProfiles.ToDictionaryAsync(x => x.Id, x => x.Name);
        }


        public virtual async Task<NotificationProfileModelDto> GetNotificationProfile(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty) throw new IdNullOrEmptyException();

            var entity = await _notificationsContext.NotificationProfiles
                .Include(x => x.NotificationEvents)
                .ThenInclude(x => x.Event)
                .Include(x => x.NotificationUsers)
                .FirstAsync(x => x.Id == id);

            if (entity == null) throw new NotFoundException(typeof(NotificationProfile).Name, $"{id}");

            var result = new NotificationProfileModelDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Events = entity.NotificationEvents.Select(x => x.Event.Id).ToList(),
                Users = entity.NotificationUsers.Select(x => x.UserId).ToList()
            };

            return result;
        }


        public virtual async Task<List<NotificationProfileModelDto>> GetNotificationProfilesList()
        {
            var entity = await _notificationsContext.NotificationProfiles
                .Include(x => x.NotificationUsers).ToListAsync();

            var result = new List<NotificationProfileModelDto>();

            foreach (var item in entity)
            {
                var modelDto = new NotificationProfileModelDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Users = item.NotificationUsers.Select(x => x.UserId).ToList()
                };
                result.Add(modelDto);
            }
            return result;
        }


        public virtual async Task UpdateNotificationProfile(NotificationProfileModelDto dto)
        {
            if (dto.Id == Guid.Empty) throw new IdNullOrEmptyException();

            var entity = await _notificationsContext.NotificationProfiles.FindAsync(dto.Id);

            if (entity == null) throw new NotFoundException(typeof(NotificationProfile).Name, $"{dto.Id}");


            entity.Name = dto.Name;

            #region NotificationUsers

            var notificationUsersInDb = await
                _notificationsContext.NotificationUsers.Where(x => x.NotificationProfileId == entity.Id)
                    .Select(x => x.UserId).ToListAsync();

            var notificationUsersToAdd = dto.Users.Except(notificationUsersInDb).ToList();
            var notificationUsersToDelete = notificationUsersInDb.Except(dto.Users).ToList();

            var notificationUsersToAddEntity = notificationUsersToAdd.
                Select(userId => new NotificationUser() { UserId = userId, NotificationProfileId = entity.Id }).ToList();

            var notificationUsersToDeleteEntity =
                _notificationsContext.NotificationUsers.Where(x => notificationUsersToDelete.Contains(x.UserId));

            _notificationsContext.NotificationUsers.RemoveRange(notificationUsersToDeleteEntity);
            await _notificationsContext.NotificationUsers.AddRangeAsync(notificationUsersToAddEntity);

            #endregion

            #region Events

            var notificationEvents = await _notificationsContext.NotificationEvents
                .Where(x => x.NotificationProfileId == entity.Id)
                .Select(x => x.EventId).ToListAsync();

            var notificationEventsToAdd = dto.Events.Except(notificationEvents).ToList();
            var notificationEventsToDelete = notificationEvents.Except(dto.Events).ToList();

            var notificationEventsToAddEntity = notificationEventsToAdd.Select(eventId => new NotificationEvent()
            {
                EventId = eventId,
                NotificationProfileId = entity.Id
            }).ToList();


            var notificationEventsToDeleteEntity =
                _notificationsContext.NotificationEvents.Where(x => notificationEventsToDelete.Contains(x.EventId));

            _notificationsContext.NotificationEvents.RemoveRange(notificationEventsToDeleteEntity);
            await _notificationsContext.AddRangeAsync(notificationEventsToAddEntity);


            #endregion

            await _notificationsContext.SaveChangesAsync();
        }


        public virtual async Task DeleteNotificationProfile(Guid? id)
        {
            if (!id.HasValue || id == Guid.Empty) throw new IdNullOrEmptyException();

            var entity = await _notificationsContext.NotificationProfiles.FindAsync(id);

            if (entity == null) throw new NotFoundException(typeof(NotificationProfile).Name, $"{id}");

            _notificationsContext.Remove(entity);
            await _notificationsContext.SaveChangesAsync();
        }

    }
}