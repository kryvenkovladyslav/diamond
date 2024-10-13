using Diamond.Jobs.Abstract;
using Diamond.Jobs.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs
{
    public class QueuedJobsHostedService<TJob, TJobItem, TJobResult, TIdentifier> : BackgroundService
        where TJob : Job<TIdentifier>
        where TJobItem : JobItem
        where TJobResult : JobResult<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected virtual IServiceProvider ServiceProvider { get; private init; }

        protected virtual IBackgroundJobQueue<TJobItem> BackgroundJobQueue { get; private init; }

        public QueuedJobsHostedService(IBackgroundJobQueue<TJobItem> backgroundJobQueue, IServiceProvider serviceProvider)
        {
            this.BackgroundJobQueue = backgroundJobQueue ?? throw new ArgumentNullException(nameof(backgroundJobQueue));
            this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken = default) => 
            await this.StartBackgroundProcessingAsync(stoppingToken);
        
        protected virtual bool IsValidJobStatus(TJob job) => job.Status == JobStatus.Started;
        
        protected virtual async Task StartBackgroundProcessingAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var jobItem = await this.BackgroundJobQueue.ReadJobAsync(cancellationToken);

                using var scope = this.ServiceProvider.CreateScope();
                using var jobManager = scope.ServiceProvider.GetRequiredService<IJobManager<TJob, TJobResult>>();

                if (jobItem == null) continue;

                var job = await jobManager.FindJobAsync(jobItem.Identifier);

                if (job == null) continue;

                var resultJobStatus = JobStatus.Completed;

                try
                {
                    if (!this.IsValidJobStatus(job)) continue;

                    var providedExecutionPlan = jobItem.ExecutionPlan;
                    var result = await providedExecutionPlan.ExecuteAsync();

                    if (result != null)
                    {
                        await jobManager.CreateJobResultAsync(jobItem.Identifier, result);
                    }
                }
                catch (Exception)
                {
                    resultJobStatus = JobStatus.Failed;
                }
                finally
                {
                    await jobManager.UpdateJobStatusAsync(job, resultJobStatus);
                }
            }
        }
    }
}