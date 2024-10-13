using Diamond.Jobs.Abstract;
using Diamond.Jobs.DataAccess;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs.EntityFrameworkStores
{
    public class JobResultStore<TContext, TJob, TJobResult, TIdentifier> : BaseJobResultStore<TJobResult, TIdentifier>
        where TJob : Job<TIdentifier>
        where TJobResult : JobResult<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TContext : JobDatabaseContext<TJob, TJobResult, TIdentifier>
    {
        protected virtual TContext Context { get; private init; }

        protected virtual EntityDbSet<TJobResult, TIdentifier> EntityDbSet { get; private init; }

        public JobResultStore(TContext context) : base(false)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.EntityDbSet = new EntityDbSet<TJobResult, TIdentifier>(this.Context.Set<TJobResult>());
        }

        public override async Task<string> CreateResultForJobAsync(TJobResult result, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(result, nameof(result));

            var identifier = await this.EntityDbSet.CreateNewEntityAsync(result, cancellationToken);
            await this.Context.SaveChangesAsync(cancellationToken);

            return this.ConvertIdentifierToString(identifier)!;
        }

        public override async Task<TJobResult?> FindJobResultByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentException.ThrowIfNullOrEmpty(identifier, nameof(identifier));

            var convertedIdentifier = this.ConvertIdentifierFromString(identifier);
            return await this.EntityDbSet.FindByIdentifierAsync(convertedIdentifier!, cancellationToken);
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.Context.Dispose();
            base.Dispose();
        }
    }
}