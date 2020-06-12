using Gear.Notifications.Domain.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Notifications.Infrastructure.Persistance.ModelsConfiguration
{
    public class NotificationUserConfig : IEntityTypeConfiguration<NotificationUser>
    {
        public void Configure(EntityTypeBuilder<NotificationUser> builder)
        {
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.NotificationProfileId)
                .IsRequired();

            builder.HasOne(x => x.NotificationProfile);

            builder.HasKey(nu => new {nu.UserId, nu.NotificationProfileId});
        }
    }
}
