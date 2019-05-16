namespace Zupan.CodeReview.WebApi
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Filters;
    using System;
    using Microsoft.AspNetCore.Mvc;
    using System.IO;
    using NSwag.AspNetCore;
    using System.Reflection;
    using NSwag.SwaggerGeneration.Processors.Security;
    using NSwag;
    using Zupan.CodeReview.IoC;

    /// <summary>
    /// Api Startup
    /// </summary>
    public partial class Startup
    {
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Auth.SetAppParametersForAuth(Configuration);
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                config.Filters.Add(new ValidateAttributes());
                config.Filters.Add(new ApiExceptionFilter());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true;
                    options.SuppressMapClientErrors = true;

                    options.ClientErrorMapping[404].Link =
                        "https://httpstatuses.com/404";
                })
                   .AddJsonOptions
                   (
                       opt =>
                       {
                           opt.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                           opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                       }
                   )
                .AddControllersAsServices();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowDevOriginsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.ConfigureApiAuthorization();

            services.AddSwaggerDocument();

            var connectionString = Configuration.GetConnectionString("MimsDB");
            var appGuid = Configuration.GetValue<string>("AppGuid");
            var brokerUri = Configuration.GetValue<string>("BrokerUri");
            var brokerUsername = Configuration.GetValue<string>("BrokerUsername");
            var brokerPassword = Configuration.GetValue<string>("BrokerPassword");

            return services.ConfigureAutoFacDependencyInjectionWebApi(connectionString, appGuid, brokerUri, brokerUsername, brokerPassword);

        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <param name="authorizationService">The authorization service.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowDevOriginsPolicy");

            app.ConfigureAuthentication(Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwaggerUi3(typeof(Startup).GetTypeInfo().Assembly, swaggerSettings =>
            {
                swaggerSettings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));

                swaggerSettings.GeneratorSettings.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("Bearer", new SwaggerSecurityScheme
                    {
                        Type = SwaggerSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        In = SwaggerSecurityApiKeyLocation.Header
                    }));
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}