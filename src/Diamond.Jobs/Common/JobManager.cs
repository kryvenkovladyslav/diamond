using Diamond.Jobs.Abstract;
using Diamond.Jobs.Contracts;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Diamond.Jobs
{
    public class JobManager<TJob, TJobResult, TIdentifier> : IJobManager<TJob, TJobResult>
        where TJob : Job<TIdentifier>, new()
        where TJobResult : JobResult<TIdentifier>, new()
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected bool IsDisposed { get; private set; }

        protected IJobStore<TJob> JobStore { get; private init; }

        protected IJobResultStore<TJobResult> JobResultStore { get; private init; }

        protected IBackgroundJobQueue<JobItem> BackgroundJobQueue { get; private init; }

        public JobManager(IJobStore<TJob> jobStore,
            IJobResultStore<TJobResult> jobResultStore,
            IBackgroundJobQueue<JobItem> backgroundJobQueue)
        {
            this.IsDisposed = false;
            this.JobStore = jobStore ?? throw new ArgumentNullException(nameof(jobStore));
            this.JobResultStore = jobResultStore ?? throw new ArgumentNullException(nameof(jobResultStore));
            this.BackgroundJobQueue = backgroundJobQueue ?? throw new ArgumentNullException(nameof(backgroundJobQueue));
        }

        public virtual async Task<string> CreateDefaultExecutableJobAsync(IExecutableJob executableJob) =>
            await this.CreateDefaultExecutableJobAsync(null, executableJob);

        public virtual async Task<string> CreateJobResultAsync(TJobResult jobResult) =>
            await this.JobResultStore.CreateResultForJobAsync(jobResult);

        public virtual async Task<TJob?> FindJobAsync(string jobIdentifier) =>
            await this.JobStore.FindJobByIdentifierAsync(jobIdentifier);

        public virtual async Task<string> CreateJobResultAsync(string jobIdentifier, object executionResult) =>
            await this.CreateJobResultAsync(new TJobResult
            {
                JsonResult = executionResult,
                Identifier = (TIdentifier)TypeDescriptor.GetConverter(typeof(TIdentifier)).ConvertFromInvariantString(jobIdentifier)!
            });

        public virtual async Task UpdateJobStatusAsync(TJob job, JobStatus status)
        {
            await this.JobStore.SetJobStatusAsync(job, status);
            await this.JobStore.UpdateAsync(job);
        }

        public virtual async Task<string> CreateDefaultExecutableJobAsync(TJob? job, IExecutableJob executableJob)
        {
            try
            {
                var jobIdentifier = await this.JobStore.CreateAsync(job ?? new TJob());
                await this.BackgroundJobQueue.WriteJobAsync(new JobItem(jobIdentifier, executableJob));
                return jobIdentifier;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(exception.Message, exception);
            }
        }

        public virtual async Task<TJobResult?> FindJobResultAsync(string identifier)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(identifier), nameof(identifier));

            var requiredJobResult = await this.JobResultStore.FindJobResultByIdentifierAsync(identifier);
            return requiredJobResult;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                this.JobStore.Dispose();
                this.JobResultStore.Dispose();

                this.IsDisposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}