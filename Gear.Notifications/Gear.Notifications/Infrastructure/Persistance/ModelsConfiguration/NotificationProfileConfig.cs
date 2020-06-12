using Gear.Notifications.Domain.BaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Notifications.Infrastructure.Persistance.ModelsConfiguration
{
    public class NotificationProfileConfig : IEntityTypeConfiguration<NotificationProfile>
    {
        public void Configure(EntityTypeBuilder<NotificationProfile> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasMany(x => x.NotificationEvents)
                .WithOne(x => x.NotificationProfile)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.NotificationEvents)
                .WithOne(x => x.NotificationProfile)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
