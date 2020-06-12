using Gear.Notifications.Domain.BaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Notifications.Infrastructure.Persistance.ModelsConfiguration
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.EventName)
                .HasMaxLength(300)
                .IsRequired();

            builder.HasOne(x => x.HtmlEventMarkup)
                .WithOne(x => x.Event);

            builder.HasOne(y => y.HtmlEventMarkup)
                .WithOne(x => x.Event)
                .HasForeignKey<HtmlEventMarkup>(x => x.EventId);

        }
    }
}
