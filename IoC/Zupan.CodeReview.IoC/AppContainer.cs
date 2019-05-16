using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Zupan.CodeReview.IoC
{
    public static class AppContainer
    {
        public static IContainer ApplicationContainer { get; set; }

        public static IServiceProvider ConfigureAutoFacDependencyInjectionWebApi(this IServiceCollection services, string connectionString, string appGuid, string brokerUri, string brokerUsername, string brokerPassword)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new WebApiModule()
            {
                ConnectionString = connectionString,
                AppGuid = appGuid,
                BrokerUri = brokerUri,
                BrokerPassword = brokerPassword,
                BrokerUsername = brokerUsername
            });
            builder.Populate(services);
            ApplicationContainer = builder.Build();
            var bc = ApplicationContainer.Resolve<IBusControl>();
            bc.Start();
            return new AutofacServiceProvider(ApplicationContainer);
        }
    }
}
