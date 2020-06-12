using Gear.Notifications.Domain.BaseModels;
using Gear.Notifications.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Gear.Notifications.Infrastructure.Persistance
{
    public class GearNotificationsContext : DbContext
    {
        public GearNotificationsContext(DbContextOptions<GearNotificationsContext> options) : base(options)
        {
            //
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<HtmlEventMarkup> HtmlEventMarkups { get; set; }

        public DbSet<NotificationProfile> NotificationProfiles { get; set; }

        public DbSet<NotificationEvent> NotificationEvents { get; set; }

        public DbSet<NotificationUser> NotificationUsers { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GearNotificationsContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
