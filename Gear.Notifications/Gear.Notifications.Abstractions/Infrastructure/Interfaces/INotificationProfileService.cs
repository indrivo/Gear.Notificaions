using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;

namespace Gear.Notifications.Abstractions.Infrastructure.Interfaces
{
    public interface INotificationProfileService
    {
        bool CheckIfProfileExists(Guid profileId);


        Task CreateNotificationProfile(NotificationProfileModelDto dto);

        /// <summary>
        /// Key -> Id
        /// String -> Name
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<Guid,string>> GetAllNotificationProfiles();


        Task<NotificationProfileModelDto> GetNotificationProfile(Guid? id);


        Task<List<NotificationProfileModelDto>> GetNotificationProfilesList();


        Task UpdateNotificationProfile(NotificationProfileModelDto dto);


        Task DeleteNotificationProfile(Guid? id);

    }
}
