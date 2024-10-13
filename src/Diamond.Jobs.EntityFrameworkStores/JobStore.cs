using Diamond.Jobs.Abstract;
using Diamond.Jobs.DataAccess;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs.EntityFrameworkStores
{
    public class JobStore<TContext, TJob, TJobResult, TIdentifier> : BaseJobStore<TJob, TIdentifier>
        where TJob : Job<TIdentifier>
        where TJobResult : JobResult<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TContext : JobDatabaseContext<TJob, TJobResult, TIdentifier>
    {
        protected virtual TContext Context { get; private init; }

        protected virtual EntityDbSet<TJob, TIdentifier> EntityDbSet { get; private init; }

        public JobStore(TContext context) : base(false)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.EntityDbSet = new EntityDbSet<TJob, TIdentifier>(this.Context.Set<TJob>());
        }

        public override async Task<string> CreateAsync(TJob job, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(job, nameof(job));

            var identifier = await this.EntityDbSet.CreateNewEntityAsync(job, cancellationToken);
            await this.Context.SaveChangesAsync(cancellationToken);

            return this.ConvertIdentifierToString(identifier)!;
        }

        public override async Task<TJob?> FindJobByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentException.ThrowIfNullOrEmpty(identifier, nameof(identifier));

            var convertedIdentifier = this.ConvertIdentifierFromString(identifier);
            return await this.EntityDbSet.FindByIdentifierAsync(convertedIdentifier!, cancellationToken);
        }

        public override async Task UpdateAsync(TJob job, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.ThrowIfDisposed();
            ArgumentNullException.ThrowIfNull(job, nameof(job));

            this.EntityDbSet.Update(job);
            await this.Context.SaveChangesAsync(cancellationToken);
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