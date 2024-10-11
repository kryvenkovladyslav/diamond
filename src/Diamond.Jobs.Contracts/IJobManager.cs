using Diamond.Jobs.Abstract;
using System;
using System.Threading.Tasks;

namespace Diamond.Jobs.Contracts
{
    public interface IJobManager<TJob, TJobResult> : IDisposable
        where TJob : class
        where TJobResult : class
    {
        public Task UpdateJobStatusAsync(TJob job, JobStatus status);

        public Task<string> CreateJobResultAsync(string jobIdentifier, object executionResult);

        public Task<string> CreateJobResultAsync(TJobResult jobResult);

        public Task<string> CreateDefaultExecutableJobAsync(IExecutableJob executableJob);

        public Task<string> CreateDefaultExecutableJobAsync(TJob? job, IExecutableJob executableJob);

        public Task<TJobResult?> FindJobResultAsync(string identifier);

        public Task<TJob?> FindJobAsync(string jobIdentifier);
    }
}