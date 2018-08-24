// Copyright (c) Microsoft. All rights reserved.

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EdgeHub.Config;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EdgeHubTriggerCsharp
{
    using System;
    using System.Collections.Generic;
    using Functions.Samples;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var typeLocator = new SamplesTypeLocator(
                typeof(EdgeHubSamples));

            IHostBuilder builder = new HostBuilder()
               .UseEnvironment("Development")

               .ConfigureWebJobs(webJobsBuilder =>
               {
                   // TEMP - remove setting host id once https://github.com/Azure/azure-webjobs-sdk/issues/1802 is fixed
                   webJobsBuilder.UseHostId("cead61-62cf-47f4-93b4-6efcded6")
                       .AddEdge();
               })
               .ConfigureLogging(b =>
               {
                   b.SetMinimumLevel(LogLevel.Debug);
                   b.AddConsole();
               })
               .ConfigureServices(s =>
               {
                   s.TryAddSingleton<ITypeLocator>(typeLocator);
               })
               .UseConsoleLifetime();

            var host = builder.Build();
            using (host)
            {
                host.RunAsync().Wait();
            }
        }
    }

    class SamplesTypeLocator : ITypeLocator
    {
        Type[] types;

        public SamplesTypeLocator(params Type[] types)
        {
            this.types = types;
        }

        public IReadOnlyList<Type> GetTypes()
        {
            return this.types;
        }
    }
}
