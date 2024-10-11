using System;

namespace Diamond.Jobs.Abstract
{
    public abstract class Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        public virtual TIdentifier Identifier { get; init; } = default!;
    }
}