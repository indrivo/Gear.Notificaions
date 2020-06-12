using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gear.Notifications.Abstractions.Infrastructure.Resources.Dtos;

namespace Gear.Notifications.Abstractions.Infrastructure.Interfaces
{
    public interface IHtmlEventMarkupService
    {
        Task CreateHtmlEventMarkup(HtmlEventMarkupModelDto model);

        /// <summary>
        /// Key -> EntityId
        /// String -> Name
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<Guid, string>> GetAllEventMarkUps();

        Task<HtmlEventMarkupModelDto> GetHtmlEventMarkup(Guid? markupId);

        Task<HtmlEventMarkupModelDto> GetHtmlEventMarkupByEvent(Guid? eventId);

        Task UpdateHtmlEventMarkup(HtmlEventMarkupModelDto model);

        Task DeleteHtmlEventMarkup(Guid? id);

        Task AssignMarkupToEvent(Guid? eventId, Guid? markupId);
    }
}
