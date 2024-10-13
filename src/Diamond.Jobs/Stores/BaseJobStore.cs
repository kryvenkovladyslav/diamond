using Diamond.Jobs.Abstract;
using Diamond.Jobs.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs
{
    public abstract class BaseJobStore<TJob, TIdentifier> : BaseStore<TIdentifier>, IJobStore<TJob>
        where TJob : Job<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        public BaseJobStore(bool isDisposed) : base(isDisposed) { }

        public Task<string> GetJobIdentifierAsync(TJob job, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(job, nameof(job));

            var convertedIdentifier = this.ConvertIdentifierToString(job.Identifier);
            return Task.FromResult(convertedIdentifier!);
        }

        public Task<JobStatus> GetJobStatusAsync(TJob job, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(job, nameof(job));

            return Task.FromResult(job.Status);
        }

        public Task SetJobStatusAsync(TJob job, JobStatus status, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(job, nameof(job));

            job.Status = status;
            return Task.CompletedTask;
        }

        public abstract Task<TJob?> FindJobByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);

        public abstract Task<string> CreateAsync(TJob job, CancellationToken cancellationToken = default);

        public abstract Task UpdateAsync(TJob job, CancellationToken cancellationToken = default);
    }
}