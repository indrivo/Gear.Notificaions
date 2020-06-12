namespace Gear.Notifications.Abstractions.Infrastructure.Resources.Settings
{
    public class NotificationServiceSettings
    {

        public string Host { get; set;}

        public bool EnableSsl { get; set;}

        public int Port { get; set;}

        /// <summary>
        /// Credentials field.
        /// </summary>
        public string UserName { get; set;}

        /// <summary>
        /// Credentials field.
        /// </summary>
        public string UserPass { get; set;}

    }
}
