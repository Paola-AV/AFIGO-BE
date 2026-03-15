using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Infraestructure.Jobs;
using Quartz;

namespace AfigoBackend.Infraestructure.Extensions
{
    public static class QuartzExtensions
    {
        public static IServiceCollection AddSyncScheduler(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("SyncJob");

                q.AddJob<SyncJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("SyncJob-trigger")
                    .WithCronSchedule(
                        "0 20 21 * * ?", // Todos los días a las 2:00 AM
                        x => x.InTimeZone(
                            TimeZoneInfo.FindSystemTimeZoneById("America/Costa_Rica")
                        )
                    )
                );
            });

            // WaitForJobsToComplete: si la app se apaga y el job está corriendo,
            // espera a que termine en lugar de cortarlo bruscamente
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
