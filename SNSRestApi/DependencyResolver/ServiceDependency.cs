using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SNSRestApi.Data.Model.Configuration;
using SNSRestApi.Policy;
using SNSRestApi.Service;
using SNSRestApi.Service.SMSChannel;

namespace SNSRestApi.DependencyResolver
{
    public static class ServiceDependency
    {
        private static void ConfigureServices(IServiceCollection services)
        {
           
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            services.Configure<Notification>(configuration.GetSection("Notification"));
            services.Configure<Policies>(configuration.GetSection("Policies"));
            services.Configure<ServiceConfiguration>(configuration.GetSection("ServiceConfiguration"));
            
            
            services.AddTransient<IReceiver, Receiver>();
            services.AddHttpClient<ISendMessage, SendMessage>()
                .AddPolicyHandler(PolicyDefined.GetRetryPolicy());

        }
        
        public static void Init()
        {
            if (InternalServiceProvider != null)
                throw new InvalidOperationException($"{nameof(ServiceDependency)} already initialized");
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            InternalServiceProvider = serviceProvider;

        }
        
        public static ServiceProvider InternalServiceProvider { private set; get; }
    }
}