using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using Gear.Notifications.Infrastructure.Persistance;
using Gear.Notifications.Service.DomainServices;
using Gear.Notifications.Service.NotificationServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gear.Notifications.Infrastructure.Resolvers
{
    public static class RensonNotificationServiceResolver
    {
        public static IServiceCollection RensonNotificationResolver(this IServiceCollection services, string connectionString, string migrationAssembly)
        {
            services.AddDbContext<GearNotificationsContext>(options => options.UseSqlServer(
                connectionString, b => b.MigrationsAssembly(migrationAssembly)));

            services.AddScoped<INotificationService, RensonNotificationService>();

            services.AddTransient<IEventService, EventService>();
            services.AddTransient<INotificationProfileService, NotificationProfileService>();
            services.AddTransient<IHtmlEventMarkupService, HtmlEventMarkupService>();

            return services;
        }
    }
}
