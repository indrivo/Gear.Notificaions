using System;

namespace Gear.Notifications.Abstractions.Domain
{
    /// <summary>
    /// Class containing the redirection data for the message
    /// TODO : Add message redirect link for other frameworks in future versions
    /// </summary>
    public class MessageRedirectAction
    {
        /// <summary>
        /// MVC redirect Controller
        /// </summary>
        public string Controller { get; set; } = "Home";

        /// <summary>
        /// MVC redirect Action
        /// </summary>
        public string Action { get; set; } = "Index";

        /// <summary>
        /// Id of the entity to redirect to 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Additional data if required
        /// Should be inserted a JSON
        /// On ui to deserialize
        /// </summary>
        public string AdditionalData { get; set; }
    }
}