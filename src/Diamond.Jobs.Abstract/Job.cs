using System;

namespace Diamond.Jobs.Abstract
{
    public class Job<TIdentifier> : Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        public virtual TIdentifier RequestID { get; set; } = default!;

        public virtual DateTime CreatedAt { get; set; }

        public virtual JobStatus Status { get; set; }
    }
}