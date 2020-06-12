using Gear.Notifications.Domain.BaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Notifications.Infrastructure.Persistance.ModelsConfiguration
{
    public class HtmlEventMarkupConfig : IEntityTypeConfiguration<HtmlEventMarkup>
    {
        public void Configure(EntityTypeBuilder<HtmlEventMarkup> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.ChangesMarkup)
                .IsRequired();

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Subtitle)
                .IsRequired();
        }
    }
}
