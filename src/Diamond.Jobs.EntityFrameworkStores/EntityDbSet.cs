using Diamond.Jobs.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Diamond.Jobs.EntityFrameworkStores
{
    public class EntityDbSet<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        protected virtual DbSet<TEntity> EntitySet { get; private init; }

        public EntityDbSet(DbSet<TEntity> entitySet) => 
            this.EntitySet = entitySet ?? throw new ArgumentNullException(nameof(entitySet));
        
        public virtual ValueTask<TEntity?> FindByIdentifierAsync(TIdentifier identifier, CancellationToken cancellationToken = default) =>
            this.EntitySet.FindAsync([identifier], cancellationToken);
        
        public virtual async Task<TIdentifier> CreateNewEntityAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = await this.EntitySet.AddAsync(entity, cancellationToken);
            return entry.Entity.Identifier;
        }

        public virtual TIdentifier Update(TEntity entity)
        {
            var entry = this.EntitySet.Update(entity);
            return entry.Entity.Identifier;
        }
    }
}