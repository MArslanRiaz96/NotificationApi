using Customer.Manager.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Manager
{
    public static class ServiceRegistration
    {
        public static void AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<INotificationManager, NotificationManager>();
        }
    }
}