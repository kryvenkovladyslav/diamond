using Diamond.Jobs.Abstract;
using Diamond.Jobs.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs
{
    public abstract class BaseJobResultStore<TJobResult, TIdentifier> : BaseStore<TIdentifier>, IJobResultStore<TJobResult>
        where TJobResult : JobResult<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        public BaseJobResultStore(bool isDisposed) : base(isDisposed) { }

        public abstract Task<string> CreateResultForJobAsync(TJobResult result, CancellationToken cancellationToken = default);

        public abstract Task<TJobResult?> FindJobResultByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
    }
}