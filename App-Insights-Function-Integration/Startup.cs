using System;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.QuickPulse;
using Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.Extensions.Options;
using System.Linq;

[assembly: FunctionsStartup(typeof(App_Insights_Function_Integration.Startup))]

namespace App_Insights_Function_Integration

{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<TelemetryConfiguration>()
     .Configure<IEnumerable<ITelemetryModuleConfigurator>, IEnumerable<ITelemetryModule>>((telemetryConfig, configurators, modules) =>
     {
         // Run through the registered configurators
         foreach (var configurator in configurators)
         {
             ITelemetryModule telemetryModule = modules.FirstOrDefault((module) => module.GetType() == configurator.TelemetryModuleType);
             if (telemetryModule != null)
             {
                 configurator.Configure(telemetryModule);
             }
         }
     });

            //builder.Services.ConfigureTelemetryModule<QuickPulseTelemetryModule>((module, o) => module.QuickPulseServiceEndpoint = "https://quickpulse.applicationinsights.us/QuickPulseService.svc");

            builder.Services.AddSingleton<IApplicationIdProvider>(_ => new ApplicationInsightsApplicationIdProvider() { ProfileQueryEndpoint = "https://dc.applicationinsights.azure.cn/api/profiles/{0}/appId" });
            builder.Services.AddSingleton<ITelemetryChannel>(s =>
            {
                // HACK: Need to force the options factory to run somewhere so it'll run through our Configurators.
                var ignore = s.GetService<IOptions<TelemetryConfiguration>>().Value;

                return new ServerTelemetryChannel { EndpointAddress = "https://dc.applicationinsights.azure.cn/v2/track" };
            });


        }
    }
}