using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core.Enrichers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Enrichers;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;

namespace common.logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger => (context, Loggerconfiguration) =>
        {

            var env = context.HostingEnvironment;
            Loggerconfiguration.MinimumLevel.Information()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
            .WriteTo.Console();

            if (env.IsDevelopment())
            {
                Loggerconfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
                Loggerconfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                Loggerconfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
                Loggerconfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
            }

            //to do elastic search configratoins

            var elsticurl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

            if(!string.IsNullOrEmpty(elsticurl))
            {
                Loggerconfiguration.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elsticurl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "new-Ecommerce-logs-{0:yyy.mm.dd}",
                    AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv8,
                    MinimumLogEventLevel = LogEventLevel.Debug
                });
            }
        };
    }
}
