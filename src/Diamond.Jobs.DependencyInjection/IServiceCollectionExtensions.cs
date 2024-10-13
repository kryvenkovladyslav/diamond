using Diamond.Jobs.Abstract;
using Diamond.Jobs.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Diamond.Jobs.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static BackgroundJobsBuilder AddBackgroundJobProcessing<TJob, TJobResult, TIdentifier>(this IServiceCollection services)
            where TJob : Job<TIdentifier>, new()
            where TJobResult : JobResult<TIdentifier>, new()
            where TIdentifier : IEquatable<TIdentifier>
        {
            services.AddSingleton<IBackgroundJobQueue<JobItem>, BackgroundJobQueue<JobItem>>();
            services.AddHostedService<QueuedJobsHostedService<TJob, JobItem, TJobResult, TIdentifier>>();
            services.AddScoped<IJobManager<TJob, TJobResult>, JobManager<TJob, TJobResult, TIdentifier>>();

            return new BackgroundJobsBuilder
            {
                Services = services,
                JobType = typeof(TJob),
                JobResultType = typeof(TJobResult),
                JobIdentifierType = typeof(TIdentifier)
            };
        }
    }
}