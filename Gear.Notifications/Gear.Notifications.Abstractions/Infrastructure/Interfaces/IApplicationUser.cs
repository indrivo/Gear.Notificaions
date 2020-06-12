using System;

namespace Gear.Notifications.Abstractions.Infrastructure.Interfaces
{
    public interface IApplicationUser
    {
        Guid Id { get; set; }

        string Email { get; set; }
    }
}
