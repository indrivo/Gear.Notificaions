using System;

namespace Gear.Notifications.Abstractions.Common.Exceptions
{
    public class IdNullOrEmptyException : Exception
    {
        public IdNullOrEmptyException() : base("The Id you provided was null")
        {
        }
    }
}
