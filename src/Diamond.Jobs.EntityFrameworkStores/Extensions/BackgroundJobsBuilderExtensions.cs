using Diamond.Jobs.Contracts;
using Diamond.Jobs.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Jobs.EntityFrameworkStores
{
    public static class BackgroundJobsBuilderExtensions
    {
        public static IServiceCollection AddEntityFrameworkJobStores<TContext>(this BackgroundJobsBuilder builder)
        {
            var jobStoreInterfaceType = typeof(IJobStore<>).MakeGenericType(builder.JobType);
            var jobResultInterfaceStoreType = typeof(IJobResultStore<>).MakeGenericType(builder.JobResultType);

            var jobStoreImplementationType = typeof(JobStore<,,,>)
                .MakeGenericType(typeof(TContext), builder.JobType, builder.JobResultType, builder.JobIdentifierType);

            var jobResultStoreImplementationType = typeof(JobResultStore<,,,>)
                .MakeGenericType(typeof(TContext), builder.JobType, builder.JobResultType, builder.JobIdentifierType);

            builder.Services.AddScoped(jobResultInterfaceStoreType, jobResultStoreImplementationType);
            builder.Services.AddScoped(jobStoreInterfaceType, jobStoreImplementationType);

            return builder.Services;
        }
    }
}