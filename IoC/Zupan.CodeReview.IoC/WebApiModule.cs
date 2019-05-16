using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.IoC
{
    using System;
    using Autofac;
    using AutoMapper;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Autofac.Core;
    using MassTransit;
    using MassTransit.Logging;
    using Serilog;
    using Zupan.CodeReview.Repository.EF.Contexts;
    using Zupan.CodeReview.UnitOfWork;
    using Zupan.CodeReview.Services.Modbus;
    using Zupan.CodeReview.Services.Core;
    using Zupan.CodeReview.Services.Interfaces.Core;
    using Zupan.CodeReview.Services.Interfaces.Modbus;

    public class WebApiModule : Autofac.Module
    {
        public string ConnectionString { get; set; }
        public string AppGuid { get; set; }
        public string BrokerUri { get; set; }
        public string BrokerUsername { get; set; }
        public string BrokerPassword { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
                .As<Profile>();

            builder.Register(c => new MapperConfiguration(cfg =>
            {
                foreach (var profile in c.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>()
                .CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CoreContext>().As<CoreContext>().WithParameter("connectionString", ConnectionString).InstancePerLifetimeScope();
            builder.RegisterType<ModbusContext>().As<ModbusContext>().WithParameter("connectionString", ConnectionString).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<ModbusDeviceService>().As<IModbusDeviceService>().PropertiesAutowired().InstancePerLifetimeScope();
            builder.RegisterType<AuthorizationService>().As<IAuthorizationService>().PropertiesAutowired().InstancePerLifetimeScope();


            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(BrokerUri), h =>
                    {
                        h.Username(BrokerUsername);
                        h.Password(BrokerPassword);
                    });

                    cfg.ReceiveEndpoint(AppGuid, ec =>
                    {
                        ec.LoadFrom(context);
                    });
                });
                return busControl;
            }).SingleInstance().As<IBusControl>();

        }
    }
}
