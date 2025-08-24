using Mango.Services.EmailAPI.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Mango.Services.EmailAPI.Extention
{
    public static class ApplicationBuilderExtention
    {
        private static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }


        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplication = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            
            hostApplication.ApplicationStarted.Register(OnStart);
            hostApplication.ApplicationStopping.Register(OnStop);
            return app;
        }

        private static void OnStart()
        {
            ServiceBusConsumer?.Start();
        }

        private static void OnStop()
        {
            ServiceBusConsumer?.Stop();
        }


    }
}
