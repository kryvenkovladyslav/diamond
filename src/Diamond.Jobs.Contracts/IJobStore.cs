using Diamond.Jobs.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs.Contracts
{
    public interface IJobStore<TJob> : IDisposable
        where TJob : class
    {
        public Task<string> CreateAsync(TJob job, CancellationToken cancellationToken = default);

        public Task UpdateAsync(TJob job, CancellationToken cancellationToken = default);

        public Task<TJob?> FindJobByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);

        public Task<JobStatus> GetJobStatusAsync(TJob job, CancellationToken cancellationToken = default);

        public Task SetJobStatusAsync(TJob job, JobStatus status, CancellationToken cancellationToken = default);

        public Task<string> GetJobIdentifierAsync(TJob job, CancellationToken cancellationToken = default);
    }
}