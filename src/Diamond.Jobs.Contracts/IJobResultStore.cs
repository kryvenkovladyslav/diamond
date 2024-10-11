using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs.Contracts
{
    public interface IJobResultStore<TJobResult> : IDisposable
        where TJobResult : class
    {
        public Task<string> CreateResultForJobAsync(TJobResult result, CancellationToken cancellationToken = default);

        public Task<TJobResult?> FindJobResultByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
    }
}