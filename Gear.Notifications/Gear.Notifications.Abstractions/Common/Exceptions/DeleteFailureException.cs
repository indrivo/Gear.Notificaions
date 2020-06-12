using System;
using System.ComponentModel;

namespace Gear.Notifications.Abstractions.Common.Exceptions
{
    public class DeleteFailureException : Exception
    {
        public DeleteFailureException(string name, object key, string message)
            : base($"Deletion of entity \"{name}\" ({key}) failed. {message}")
        {
        }
    }

    public enum DeleteFailureMessages
    {
        [Description("Entity is not deletable")]
        EntityNotDeletable = 1
    }
}
