using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;

namespace Gear.Notifications.Abstractions.Infrastructure.Interfaces
{
    public interface IEventService
    {

        /// <summary>
        /// Create event
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        Task CreateEvent(string eventName);


        /// <summary>
        /// Import event from DB where event implements TYPE
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task ImportAndCreateEvents(Assembly assemblyName, Type type);


        /// <summary>
        /// Import event from assembly where event implements MediatR.INotification 
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        Task ImportAndCreateEvents(Assembly assemblyName);


        /// <summary>
        /// Create Event Range
        /// </summary>
        /// <param name="eventNames"></param>
        /// <returns></returns>
        Task AddRange(ICollection<string> eventNames);


        /// <summary>
        /// Key -> Id
        /// String -> Name
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<Guid, string>> GetAllEventsNames();


        /// <summary>
        /// Get an event Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EventDto> GetEventModel(Guid? id);


        /// <summary>
        /// Method present for events that were added by mistake or in the future
        /// needs shouldn't be really used.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteEvent(Guid? id);


        /// <summary>
        /// Update Specific event
        /// </summary>
        /// <param name="eventModel"></param>
        /// <returns></returns>
        Task UpdateEvent(EventDto eventModel);


    }
}
