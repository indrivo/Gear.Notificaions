using Gear.Notifications.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Notifications.Infrastructure.Persistance.ModelsConfiguration
{
    public class NotificationEventConfig : IEntityTypeConfiguration<NotificationEvent>
    {
        public void Configure(EntityTypeBuilder<NotificationEvent> builder)
        {
            builder.Property(x => x.EventId)
                .IsRequired();

            builder.Property(x => x.NotificationProfileId)
                .IsRequired();

            builder.HasKey(en => new {en.EventId, en.NotificationProfileId});

            builder.HasOne(x => x.NotificationProfile)
                .WithMany(x => x.NotificationEvents)
                .HasForeignKey(x => x.NotificationProfileId);

            builder.HasOne(x => x.Event)
                .WithMany(x => x.NotificationEvents)
                .HasForeignKey(x => x.EventId);
        }
    }
}
